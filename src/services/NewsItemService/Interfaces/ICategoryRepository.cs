using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Dictionary<bool, Category>> GetCategoryById(int id);
    }
}
