using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(int newsItemID);
        Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item);
    }
}
