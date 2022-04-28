using NewsItemService.DTOs;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatus newsItemStatus);
    }
}
