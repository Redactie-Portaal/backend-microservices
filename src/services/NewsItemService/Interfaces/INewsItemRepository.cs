namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(int newsItemID);
    }
}
