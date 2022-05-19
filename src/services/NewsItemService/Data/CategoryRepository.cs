using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public CategoryRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _dbContext.Categories.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (category == null)
                {
                    return new Dictionary<bool, Category>() { { false, null } };
                }
                return new Dictionary<bool, Category>() { { true, category } };
            }
            catch (Exception)
            {
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
