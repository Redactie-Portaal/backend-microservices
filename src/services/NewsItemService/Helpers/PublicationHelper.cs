using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Helpers
{
    public class PublicationHelper
    {
        public static List<PublicationDTO> ToDTO(List<Publication> publications)
        {
            List<PublicationDTO> publicationDTOs = new List<PublicationDTO>();
            foreach (Publication pub in publications)
            {
                publicationDTOs.Add(ToDTO(pub));
            }

            return publicationDTOs;
        }

        public static PublicationDTO ToDTO(Publication publication)
        {
            return new PublicationDTO()
            {
                Id = publication.Id,
                Icon = publication.Icon == null ? string.Empty : publication.Icon,
                Name = publication.Name
            };
        }

        public static Publication ToEntity(CreatePublicationDTO createPublicationDTO)
        {
            return new Publication()
            {
                Name = createPublicationDTO.Name,
                Description = createPublicationDTO.Description,
                Icon = createPublicationDTO.Icon
            };
        }
    }
}
