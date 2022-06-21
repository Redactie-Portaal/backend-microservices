using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class SourcePerson
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<NewsItem> NewsItems { get; set; }
    }
}
