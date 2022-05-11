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
                var author = new Author() { Id = 1, Name = "Robert Bever" };
                var newsItems = new List<NewsItem>() { new NewsItem() { Id = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now, Authors = new List<Author>() { author } } };
                return Task.FromResult(newsItems);
            }
            if (authorId == 2) return Task.FromResult<List<NewsItem>?>(null);
            if (authorId == 5)
            {
                var author = new Author() { Id = 1, Name = "Robert Bever" };
                var authorTwo = new Author() { Id = 5, Name = "Harold LoopDeLaInfinite" };
                var newsItems = new List<NewsItem>() {
                    new NewsItem() { Id = 1, Name = "Vogel valt uit nest.", Status = "Processing", Authors = new List<Author>() { authorTwo } },
                    new NewsItem() { Id = 2, Name = "Papegaai krijgt medaille.", Status = "Archived", Authors = new List<Author>() { author, authorTwo } }
                };
                return Task.FromResult(newsItems);
            } 

            return Task.FromResult<List<NewsItem>?>(null);
        }
    }
}
