using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository
    {
        Task<List<NewsItem>?> GetNewsItems(int authorId);
    }
}
