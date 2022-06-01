using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class MediaNewsItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string MediaId { get; set; } = string.Empty;
        public int NewsItemId { get; set; }
        public bool IsSource { get; set; }
    }
}
