using NewsItemService.Entities;
using NewsItemService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsItemService.UnitTests.Stubs
{
    public class StubNewsItemRepository : INewsItemRepository
    {
        public Task<List<NewsItem>?> GetNewsItems(int authorId)
        {
            if (authorId == 1)
            {
                var newsItems = new List<NewsItem>() { new NewsItem() { Id = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now } };
                var author = new Author() { Id = 1, Name = "Robert Bever", NewsItems = newsItems };
                return Task.FromResult(author);
            }
            if (authorId == 2) return Task.FromResult<Author>(null);
            if (authorId == 5)
            {
                var newsItems = new List<NewsItem>() {
                    new NewsItem() { Id = 1, Name = "Vogel valt uit nest.", Status = "Processing" },
                    new NewsItem() { Id = 2, Name = "Papegaai krijgt medaille.", Status = "Archived" }
                };
                var author = new Author() { Id = 5, Name = "Harold LööpDeLaInfinite", NewsItems = newsItems };
                return Task.FromResult(author);
            }

            return Task.FromResult<Author>(null);
        }
    }
}
