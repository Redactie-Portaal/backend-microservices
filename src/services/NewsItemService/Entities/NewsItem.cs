using NewsItemService.Enums;
using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class NewsItem
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public int NewsItemID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<Author> Authors { get; set; }
        public NewsItemStatus Status { get; set; }
    }
}
