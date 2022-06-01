using Microsoft.EntityFrameworkCore;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class NewsItemRepository: INewsItemRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public NewsItemRepository(NewsItemServiceDatabaseContext context, ILogger<NewsItemRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
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

            return new Dictionary<bool, string>() { { false, $"Article '{item.Title}' has been created succesfully" } };
        }

        public async Task<Dictionary<bool, NewsItem>> GetNewsItemById(int newsItemId)
        {
            try
            {
                var newsItem = await _dbContext.NewsItems.Where(a => a.Id == newsItemId).FirstOrDefaultAsync();
                if (newsItem == null)
                {
                    return new Dictionary<bool, NewsItem>() { { false, null } };
                }
                return new Dictionary<bool, NewsItem>() { { true, newsItem } };
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the NewsItem. Error message: {Message}", ex.Message);
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
