using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ISourcePersonRepository
    {
        Task<Dictionary<bool, SourcePerson>> GetSourcePerson(AddSourcePersonDTO addSourcePersonDTO);
        Task<Dictionary<bool, string>> CreateSourcePerson(SourcePerson sourcePerson);
    }
}
