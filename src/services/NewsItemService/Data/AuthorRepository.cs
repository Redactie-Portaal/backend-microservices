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

        public List<Author> Get()
        {
            var authors = _context.Authors.ToList();
            if (authors == null) throw new Exception("No authors found.");

            return authors;
        }

        public Author Get(int id) {
            var author = _context.Authors.Include("NewsItems").FirstOrDefault(a => a.Id == id);
            if (author == null) throw new Exception("Author not found.");

            return author;
        }

        public Author Post(Author author) 
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return author;
        }
    }
}