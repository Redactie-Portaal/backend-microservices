using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface INoteRepository
    {
        Task<Dictionary<bool, string>> CreateNote(Note note);
    }
}
