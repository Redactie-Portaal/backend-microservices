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
                    await Save();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new Dictionary<bool, string>() { { true, $"Article '{item.Title}' has been created succesfully" } };
        }

        public async Task<Dictionary<bool, Author>> GetAuthorById(int id)
        {
            try
            {
                var author =  await _dbContext.Authors.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (author == null)
                {
                    return new Dictionary<bool, Author>() { { false, null } };
                }
                return new Dictionary<bool, Author>() { {true, author } };
            }
            catch (Exception)
            {
                throw;
            }
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

        public Task<Dictionary<bool, string>> ChangeNewsItemStatus(int newsItemID)
        {
            throw new NotImplementedException();
        }
    }
}
