using NewsItemService.Types;
using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class Media
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public NewsItemMediaType NewsItemMediaType { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        
        public ICollection<MediaNewsItem> MediaNewsItems { get; set; }
    }
}
