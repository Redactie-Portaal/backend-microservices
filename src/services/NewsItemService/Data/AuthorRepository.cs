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
        
        public List<Author>? Get()
        {
            return _context.Authors.ToList();
        }

        public Author Get(int id)
        {
            var author = _context.Authors.Include("NewsItems").FirstOrDefault(a => a.Id == id);
            if (author == null) return null;

            return author;
        }

        public List<NewsItem>? GetNewsItems(int id, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;
            
            var newsItems = _context.NewsItems.Where(n => n.Authors.Any(a => a.Id == id)).Skip(amountToSkip).Take(pageSize).ToList();
            if (newsItems.Count <= 0) return null;

            return newsItems;
        }

        public Author Post(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return author;
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