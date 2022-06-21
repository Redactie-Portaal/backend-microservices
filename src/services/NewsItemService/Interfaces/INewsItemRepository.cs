using NewsItemService.Entities;
using NewsItemService.DTOs;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        // Task<Dictionary<bool, NewsItem>> GetNewsItemById(int newsItemId);
        Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item);
        List<NewsItem> Get(int page, int pageSize);
        NewsItem? Get(int id);

        List<NewsItem> GetBefore(DateTime date, int page, int pageSize);
        List<NewsItem> GetAfter(DateTime date, int page, int pageSize);

        List<NewsItem> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize);
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatusDTO newsItemStatus);
        // Task<NewsItem> GetNewsItemAsync(int newsItemId);
    }
}