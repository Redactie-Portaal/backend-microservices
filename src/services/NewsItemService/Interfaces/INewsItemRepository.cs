using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository
    {
        List<NewsItem> Get(int page, int pageSize);

        NewsItem Get(int id);

        NewsItem Post(NewsItem newsItem);

        List<NewsItem> GetBefore(DateTime date, int page, int pageSize);

        List<NewsItem> GetAfter(DateTime date, int page, int pageSize);

        List<NewsItem> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize);
    }
}
