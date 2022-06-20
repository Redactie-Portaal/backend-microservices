using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Dictionary<bool, Author>> GetAuthorById(int id);
        List<Author>? Get();

        Author? Get(int id);

        Author Post(Author author);

        List<NewsItem>? GetNewsItems(int id, int page, int pageSize);
    }
}
