using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ISourceLocationRepository
    {
        Task<Dictionary<bool, SourceLocation>> GetSourceLocationById(int id);
        Task<Dictionary<bool, string>> CreateSourceLocation(SourceLocation sourceLocation);
    }
}
