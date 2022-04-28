using NewsItemService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class NewsItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int NewsItemID { get; set; }
        public int PublicationID { get; set; }
        public int TagID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<Author> Authors { get; set; }
        public NewsItemStatus Status { get; set; }

        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ContactInformation { get; set; }
        public string? LocationInformation { get; set; }
        public string? Region { get; set; }
    }
}
