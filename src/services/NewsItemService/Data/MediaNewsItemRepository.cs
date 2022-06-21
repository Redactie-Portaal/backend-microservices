using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class MediaNewsItemRepository : IMediaNewsItemRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private readonly ILogger _logger;

        private bool _disposed = false;

        public MediaNewsItemRepository(NewsItemServiceDatabaseContext context, ILogger<MediaNewsItemRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<Dictionary<bool, List<MediaNewsItem>>> GetMediaNewsItemByNewsItemId(int newsItemId)
        {
            try
            {
                var mediaNewsItems = await _dbContext.MediaNewsItems.Where(a => a.NewsItemId == newsItemId).ToListAsync();
                if (mediaNewsItems == null)
                {
                    return new Dictionary<bool, List<MediaNewsItem>> () { { false, null } };
                }
                return new Dictionary<bool, List<MediaNewsItem>> () { { true, mediaNewsItems } };
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the MediaNewsItem. Error message: {Message}", ex.Message);
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
                    await _dbContext.MediaNewsItems.AddAsync(mediaNewsItem);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with saving the MediaNewsItem. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"MediaNewsItem has been created succesfully" } };
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
