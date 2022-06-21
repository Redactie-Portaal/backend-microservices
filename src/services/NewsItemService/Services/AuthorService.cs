using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<AuthorDTO>? Get() 
        {
            var authors = _authorRepository.Get();
            if (authors == null) return null;

            return AuthorHelper.ToDTO(authors);
        }

        public async Task<AuthorDTO?> Get(int id)
        {
            Dictionary<bool, Author> result = await _authorRepository.GetAuthorById(id);
            
            bool error = !result.FirstOrDefault().Key;
            if (error) return null;

            var author = result.FirstOrDefault().Value;
            
            return AuthorHelper.ToDTO(author);
        }

        public List<NewsItemDTO>? GetNewsItems(int id, int page, int pageSize)
        {
            var newsItems = _authorRepository.GetNewsItems(id, page, pageSize);
            if (newsItems == null) return null;

            return NewsItemHelper.ToDTO(newsItems, false);
        }

        public AuthorDTO Post(CreateAuthorDTO createAuthorDTO)
        {
            var author = new Author
            {
                Name = createAuthorDTO.Name
            };

            return AuthorHelper.ToDTO(_authorRepository.Post(author));
        }
    }
}
