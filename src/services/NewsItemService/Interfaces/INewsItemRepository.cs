using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository
    {
        List<NewsItem> Get();

        NewsItem Get(int id);

        NewsItem Post(NewsItem newsItem);

        List<NewsItem> GetBefore(DateTime date);

        List<NewsItem> GetAfter(DateTime date);

        List<NewsItem> GetBetween(DateTime startDate, DateTime endDate);
    }
}
