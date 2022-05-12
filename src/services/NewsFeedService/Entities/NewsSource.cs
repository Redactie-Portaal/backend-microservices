using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Entities
{
    public class NewsSource
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        //foreign keys
        public int TypeId { get; set; }
    }
}
