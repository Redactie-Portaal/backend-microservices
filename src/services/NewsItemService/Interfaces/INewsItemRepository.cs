using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository
    {
        Task<Author?> GetNewsItems(int authorId);
    }
}
