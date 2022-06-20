using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private readonly ILogger _logger;

        private bool _disposed = false;

        public CategoryRepository(NewsItemServiceDatabaseContext dbContext, ILogger<CategoryRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the Category. Error message: {Message}", ex.Message);
                throw;
            }
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
