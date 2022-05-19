using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Dictionary<bool, Author>> GetAuthorById(int id);
    }
}
