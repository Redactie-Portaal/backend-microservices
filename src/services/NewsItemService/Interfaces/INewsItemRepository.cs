using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository
    {
        Task<List<Author>> GetNewsItems(int authorId);
    }
}
