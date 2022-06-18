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
                publicationDTOs.Add(new PublicationDTO()
                {
                    Id = pub.Id,
                    Icon = pub.Icon,
                    Name  = pub.Name
                });
            }

            return publicationDTOs;
        }

    }
}
