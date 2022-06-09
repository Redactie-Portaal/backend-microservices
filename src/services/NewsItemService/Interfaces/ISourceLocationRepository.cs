using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ISourceLocationRepository
    {
        Task<Dictionary<bool, SourceLocation>> GetSourceLocation(AddSourceLocationDTO addSourceLocationDTO);
        Task<Dictionary<bool, string>> CreateSourceLocation(SourceLocation sourceLocation);
    }
}
