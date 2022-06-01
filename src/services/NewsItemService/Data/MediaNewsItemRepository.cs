using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class MediaNewsItemRepository : IMediaNewsItemRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public MediaNewsItemRepository(NewsItemServiceDatabaseContext context, ILogger<MediaNewsItemRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }

        public async Task<Dictionary<bool, MediaNewsItem>> GetMediaNewsItemById(int newsItemId)
        {
            try
            {
                var mediaNewsItem = await _dbContext.MediaNewsItems.Where(a => a.NewsItemId == newsItemId).FirstOrDefaultAsync();
                if (mediaNewsItem == null)
                {
                    return new Dictionary<bool, MediaNewsItem>() { { false, null } };
                }
                return new Dictionary<bool, MediaNewsItem>() { { true, mediaNewsItem } };
            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with retrieving the MediaNewsItem. Error message: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Dictionary<bool, string>> CreateMediaNewsItem(MediaNewsItem mediaNewsItem)
        {
            try
            {
                var duplicate = await _dbContext.MediaNewsItems.FirstOrDefaultAsync(x => x.MediaId == mediaNewsItem.MediaId);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "MediaNewsItem is already present" } };
                }
                else
                {
                    //_dbContext.Entry(mediaNewsItem.NewsItem).CurrentValues.SetValues(mediaNewsItem.NewsItem);
                    _dbContext.Entry(mediaNewsItem.NewsItem).State = EntityState.Detached;
                    await _dbContext.MediaNewsItems.AddAsync(mediaNewsItem);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with saving the MediaNewsItem. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"MediaNewsItem has been created succesfully" } };
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
