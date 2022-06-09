using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Helpers
{
    class AuthorHelper
    {
        public static AuthorDTO ToDTO(Author author)
        {
            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public static List<AuthorDTO> ToDTO(List<Author> authors)
        {
            List<AuthorDTO> authorDTOs = new List<AuthorDTO>();
            foreach (Author author in authors)
            {
                authorDTOs.Add(AuthorHelper.ToDTO(author));
            }

            return authorDTOs;
        }

        public static Author ToAuthor(AuthorDTO authorDTO)
        {
            return new Author
            {
                Id = authorDTO.Id,
                Name = authorDTO.Name
            };
        }
    }
}