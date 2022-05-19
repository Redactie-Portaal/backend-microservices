using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IAuthorRepository
    {
        List<Author> Get();

        Author Get(int id);

        Author Post(Author author);
    }
}
