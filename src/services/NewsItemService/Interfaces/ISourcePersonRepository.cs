using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ISourcePersonRepository
    {
        Task<Dictionary<bool, SourcePerson>> GetSourcePersonById(int id);
        Task<Dictionary<bool, string>> CreateSourcePerson(SourcePerson sourcePerson);
    }
}
