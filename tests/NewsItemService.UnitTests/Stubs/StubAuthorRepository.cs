using System.Collections.Generic;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.UnitTests.Stubs
{
    public class StubAuthorRepository : IAuthorRepository
    {
        public List<Author> Get()
        {
            throw new System.NotImplementedException();
        }

        public Author Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<NewsItem>? GetNewsItems(int id, int page, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public Author Post(Author author)
        {
            throw new System.NotImplementedException();
        }
    }
}