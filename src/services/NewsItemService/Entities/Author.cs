using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class Author
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public ICollection<NewsItem> NewsItems { get; set; }
    }
}
