using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsItemService.Entities
{
    public class MediaNewsItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int MediaId { get; set; }
        public Media Media { get; set; }

        public int NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }

        public bool IsSource { get; set; }
        
    }
}
