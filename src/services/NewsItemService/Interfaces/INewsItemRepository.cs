using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatusDTO newsItemStatus);
        Task<NewsItem> GetNewsItemAsync(int newsItemId);
    }
}
