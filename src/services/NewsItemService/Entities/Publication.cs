using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class Publication
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Icon { get; set; } = string.Empty;
        public ICollection<NewsItem> NewsItems { get; set; }
    }
}
