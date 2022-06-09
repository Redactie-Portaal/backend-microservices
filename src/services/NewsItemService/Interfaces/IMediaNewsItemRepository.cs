using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IMediaNewsItemRepository
    {
        Task<Dictionary<bool, MediaNewsItem>> GetMediaNewsItemById(int newsItemId);
        Task<Dictionary<bool, string>> CreateMediaNewsItem(MediaNewsItem mediaNewsItem);
    }
}
