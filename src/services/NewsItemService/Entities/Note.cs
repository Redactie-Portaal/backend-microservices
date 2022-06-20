using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class Note
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public Author Author { get; set; }
        public NewsItem NewsItem { get;set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Updated { get; set; }
    }
}
