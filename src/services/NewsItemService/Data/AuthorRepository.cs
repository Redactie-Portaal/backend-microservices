using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class AuthorRepository: IAuthorRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public AuthorRepository(NewsItemServiceDatabaseContext context, ILogger<AuthorRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }

        public async Task<Dictionary<bool, Author>> GetAuthorById(int id)
        {
            try
            {
                var author = await _dbContext.Authors.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (author == null)
                {
                    return new Dictionary<bool, Author>() { { false, null } };
                }
                return new Dictionary<bool, Author>() { { true, author } };
            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with retrieving the Author. Error message: {Message}", ex.Message);
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
