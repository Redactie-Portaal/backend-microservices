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

        public List<NewsItem> Get(int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems =  _context.NewsItems.Include("Authors").Skip(amountToSkip).Take(pageSize).ToList();

            return newsItems;
        }

        public NewsItem? Get(int id)
        {
            var newsItem = _context.NewsItems.FirstOrDefault(n => n.Id == id);

            return newsItem;
        }

        public NewsItem Post(NewsItem newsItem)
        {
            _context.NewsItems.Add(newsItem);
            _context.SaveChanges();

            return newsItem;
        }

        public List<NewsItem> GetBefore(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _context.NewsItems.Where(n => n.Created < date).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }

        public List<NewsItem> GetAfter(DateTime date, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _context.NewsItems.Where(n => n.Created > date).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }

        public List<NewsItem> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var amountToSkip = (page - 1) * pageSize;

            var newsItems = _context.NewsItems.Where(n => n.Created > startDate && n.Created < endDate).Skip(amountToSkip).Take(pageSize).Include("Authors").ToList();
            if (newsItems == null) throw new Exception($"No news items found.");

            return newsItems;
        }
    }
}
