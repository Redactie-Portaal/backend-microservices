using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {
        private readonly ILogger _logger;
        private readonly NewsItemServiceDatabaseContext _dbContext;

        private bool _disposed = false;

        public AuthorRepository(ILogger<AuthorRepository> logger, NewsItemServiceDatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
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
                _logger.LogError("There is a problem with retrieving the Author. Error message: {Message}", ex.Message);
                throw;
            }
        }
        
        public List<Author>? Get()
        {
            return _dbContext.Authors.ToList();
        }

        public List<NewsItem>? GetNewsItems(int id, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;
            
            var newsItems = _dbContext.NewsItems.Where(n => n.Authors.Any(a => a.Id == id)).Skip(amountToSkip).Take(pageSize).ToList();
            if (newsItems.Count <= 0) return null;

            return newsItems;
        }

        public Author Post(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();

            return author;
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