using NewsItemService.Entities;
using NewsItemService.Interfaces;
using NewsItemService.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsItemService.Tests.UnitTests.Stubs
{
    public class StubNewsItemRepository : INewsItemRepository
    {
        private List<NewsItem> _newsItems { get; set; }

        public StubNewsItemRepository()
        {
            var newsItemsCount = 100;
            _newsItems = new List<NewsItem>();

            for (int i = 1; i <= newsItemsCount; i++)
            {
                _newsItems.Add(new NewsItem()
                {
                    Id = i,
                    Title = $"Title: {i}",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<Author>()
                    {
                        new Author()
                        {
                            Id = 1,
                            Name = $"James"
                        }
                    },
                    Status = NewsItemStatus.Done
                });
            }
        }

        public List<NewsItem> Get(int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            return _newsItems.Skip(amountToSkip).Take(pageSize).ToList();
        }

        public NewsItem? Get(int id)
        {
            return _newsItems.FirstOrDefault(n => n.Id == id);
        }

        public List<NewsItem> GetAfter(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            return _newsItems.Skip(amountToSkip).Take(pageSize).ToList();
        }

        public List<NewsItem> GetBefore(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            return _newsItems.Skip(amountToSkip).Take(pageSize).ToList();
        }

        public List<NewsItem> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            return _newsItems.Skip(amountToSkip).Take(pageSize).ToList();
        }

        public NewsItem Post(NewsItem newsItem)
        {
            throw new NotImplementedException();
        }
    }
}
