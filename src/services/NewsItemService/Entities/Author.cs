using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<NewsItem> NewsItems { get; set; }
    }
}
