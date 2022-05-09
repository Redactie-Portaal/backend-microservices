using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Entities
{
    public class FeedLabel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Label { get; set; }

        public ICollection<FeedItem> FeedItems { get; set; }
    }
}
