using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsItemService.Entities;
using NewsItemService.Interfaces;
using NewsItemService.Types;

namespace NewsItemService.Tests.UnitTests.Stubs
{
    public class StubAuthorRepository : IAuthorRepository
    {
        private List<Author> _authors = new List<Author>()
        {
            new Author() { Id = 1, Name = "Jacob" },
            new Author() { Id = 2, Name = "Jason" }
        };

        private List<NewsItem> _newsItems;

        public StubAuthorRepository()
        {
            var newsItemsCount = 20;
            _newsItems = new List<NewsItem>();

            for (int i = 1; i <= newsItemsCount; i++)
            {
                _newsItems.Add(new NewsItem()
                {
                    Id = i,
                    Title = $"Title: {i}",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = _authors,
                    Status = NewsItemStatus.Done
                });
            }
        }

        public List<Author> Get()
        {
            return _authors;
        }

        public Author? Get(int id)
        {
            return _authors.FirstOrDefault(a => a.Id == id);
        }

        public Task<Dictionary<bool, Author>> GetAuthorById(int id)
        {
            var result = _authors.FirstOrDefault(a => a.Id == id);
            if (result == null) return Task.FromResult(new Dictionary<bool, Author>() { { false, null } });
            return Task.FromResult(new Dictionary<bool, Author>() { { true, result } });
        }

        public List<NewsItem>? GetNewsItems(int id, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            return _newsItems.Skip(amountToSkip).Take(pageSize).ToList();
        }

        public Author Post(Author author)
        {
            throw new System.NotImplementedException();
        }
    }
}