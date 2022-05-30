using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IMediaNewsItemRepository
    {
        Task<Dictionary<bool, MediaNewsItem>> GetMediaNewsItemById(int id);
        Task<Dictionary<bool, string>> CreateMediaNewsItem(MediaNewsItem mediaNewsItem);
    }
}
