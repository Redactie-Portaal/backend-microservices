using Microsoft.EntityFrameworkCore;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class NewsItemRepository : INewsItemRepository, IDisposable
    {
        private readonly ILogger _logger;
        private readonly NewsItemServiceDatabaseContext _dbContext;

        private bool _disposed = false;

        public NewsItemRepository(NewsItemServiceDatabaseContext dbContext, ILogger<NewsItemRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<NewsItem> Get(int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;
            var newsItems =  _dbContext.NewsItems.Include("Authors").Include("Publications").Skip(amountToSkip).Take(pageSize).ToList();

            return newsItems;
        }

        public NewsItem? Get(int id)
        {
            var newsItem = _dbContext.NewsItems.FirstOrDefault(n => n.Id == id);

            return newsItem;
        }

        public List<NewsItem> GetBefore(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _dbContext.NewsItems.Where(n => n.Created < date).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }

        public List<NewsItem> GetAfter(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _dbContext.NewsItems.Where(n => n.Created > date).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }

        public List<NewsItem> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _dbContext.NewsItems.Where(n => n.Created > startDate && n.Created < endDate).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }
        
        public async Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item)
        { 
            try
            {
                var duplicate = await _dbContext.NewsItems.FirstOrDefaultAsync(x => x.Title == item.Title);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create newsItem with a title that has already been used" } };
                }
                else
                {
                    await _dbContext.NewsItems.AddAsync(item);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with saving the NewsItem. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { true, $"Article '{item.Title}' has been created succesfully" } };
        }

        public async Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatusDTO newsItemStatus)
        {
            NewsItem item = await _dbContext.NewsItems.FirstOrDefaultAsync(x => x.Id == newsItemStatus.NewsItemId);

            if (item == default)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.NO_NEWSITEM" } };
            }

            if (item.Status == newsItemStatus.status)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.DUPLICATE_STATUS" } };
            }

            item.Status = newsItemStatus.status;
            item.Updated = DateTime.Now.ToUniversalTime();

            if (!_dbContext.ChangeTracker.HasChanges())
            {
                return new Dictionary<bool, string>() { { false, "STATUS.NO_CHANGES_DETECTED" } };
            }

            await _dbContext.SaveChangesAsync();
            return new Dictionary<bool, string>() { { true, "Status changed to " + newsItemStatus.status.ToString() } };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}