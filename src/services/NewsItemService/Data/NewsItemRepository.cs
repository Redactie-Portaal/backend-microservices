using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class NewsItemRepository : INewsItemRepository
    {
        private readonly NewsItemServiceDatabaseContext _context;

        public NewsItemRepository(NewsItemServiceDatabaseContext context)
        {
            _context = context;
        }

        public List<NewsItem> Get()
        {
            var newsItems =  _context.NewsItems.Include("Authors").ToList();
            if (newsItems == null) throw new Exception("No news items found.");

            return newsItems;
        }

        public NewsItem Get(int id)
        {
            var newsItem = _context.NewsItems.FirstOrDefault(n => n.Id == id);
            if (newsItem == null) throw new Exception("News item not found.");

            return newsItem;
        }

        public NewsItem Post(NewsItem newsItem)
        {
            _context.NewsItems.Add(newsItem);
            _context.SaveChanges();

            return newsItem;
        }

        public List<NewsItem> GetBefore(DateTime date)
        {
            var newsItems = _context.NewsItems.Where(n => n.Created < date).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items before {date} found.");

            return newsItems;
        }

        public List<NewsItem> GetAfter(DateTime date)
        {
            var newsItems = _context.NewsItems.Where(n => n.Created > date).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items after {date} found.");

            return newsItems;
        }

        public List<NewsItem> GetBetween(DateTime startDate, DateTime endDate)
        {
            var newsItems = _context.NewsItems.Where(n => n.Created > startDate && n.Created < endDate).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items between {startDate} and {endDate} found.");

            return newsItems;
        }
    }
}
