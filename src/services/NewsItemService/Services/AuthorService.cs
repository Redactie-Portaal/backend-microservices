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
            this._authorRepository = authorRepository;
        }

        public List<Author> Get() 
        {
            return this._authorRepository.Get();
        }

        public Author Get(int id)
        {
            return this._authorRepository.Get(id);
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
