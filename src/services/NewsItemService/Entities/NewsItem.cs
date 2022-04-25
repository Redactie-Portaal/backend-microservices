using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class NewsItem
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<Author> Authors { get; set; }
        public int Status { get; set; }
    }
}
