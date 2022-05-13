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

        public NewsItemRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatus newsItemStatus)
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
        public async Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item)
        {
            try
            {
                var duplicate = await _dbContext.NewsItems.FirstOrDefaultAsync(x => x.Title == item.Title);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create article with a title that has already been used" } };
                }
                else
                {
                    await _dbContext.NewsItems.AddAsync(item);
                    Save();
                }
            }
            catch
            {
                return new Dictionary<bool, string>() { { false, "fout" } };
            }

            return new Dictionary<bool, string>() { { true, $"Article '{item.Title}' has been created succesfully" } };
        }

        public void Save()
        {
            _dbContext.SaveChanges();
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

        public Task<Dictionary<bool, string>> ChangeNewsItemStatus(int newsItemID)
        {
            throw new NotImplementedException();
        }
    }
}
