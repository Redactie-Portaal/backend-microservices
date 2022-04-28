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

            if(item == default)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.NO_NEWSITEM" } };
            }

            item.Status = newsItemStatus.status;
            item.Updated = DateTime.Now.ToUniversalTime();

            if (item == default)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.SAVING_STATUS_FAILED" } };
            }

            await _dbContext.SaveChangesAsync();
            return new Dictionary<bool, string>() { { true, "Status changed to " + newsItemStatus.status.ToString() } };
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
