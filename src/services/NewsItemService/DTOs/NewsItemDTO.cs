using NewsItemService.Entities;
using NewsItemService.Types;

namespace NewsItemService.DTOs
{
    public class NewsItemDTO
    {
        //TODO: UPDATE MORE PROPERTIES
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<AuthorDTO> Authors { get; set; }
        public NewsItemStatus Status { get; set; }

        public List<PublicationDTO> Publications { get; set; } = new();
    }
}
