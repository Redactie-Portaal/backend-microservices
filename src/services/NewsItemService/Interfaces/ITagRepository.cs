using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface ITagRepository
    {
        Task<Dictionary<bool, Tag>> GetTagById(int id);
        Tag Post(Tag tag);
    }
}
