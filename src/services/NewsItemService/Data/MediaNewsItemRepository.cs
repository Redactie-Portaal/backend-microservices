using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class MediaNewsItemRepository : IMediaNewsItemRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public MediaNewsItemRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, MediaNewsItem>> GetMediaNewsItemById(int id)
        {
            try
            {
                var mediaNewsItem = await _dbContext.MediaNewsItems.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (mediaNewsItem == null)
                {
                    return new Dictionary<bool, MediaNewsItem>() { { false, null } };
                }
                return new Dictionary<bool, MediaNewsItem>() { { true, mediaNewsItem } };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Dictionary<bool, string>> CreateMediaNewsItem(MediaNewsItem mediaNewsItem)
        {
            try
            {
                var duplicate = await _dbContext.MediaNewsItems.FirstOrDefaultAsync(x => x.Id == mediaNewsItem.Id);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create article with a title that has already been used" } };
                }
                else
                {
                    await _dbContext.MediaNewsItems.AddAsync(mediaNewsItem);
                    await Save();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"MediaNewsItem has been created succesfully" } };
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
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
