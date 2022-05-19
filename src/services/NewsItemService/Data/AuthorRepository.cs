using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly NewsItemServiceDatabaseContext _context;

        public AuthorRepository(NewsItemServiceDatabaseContext context)
        {
            _context = context;
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
    }
}