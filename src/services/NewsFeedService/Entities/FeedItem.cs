using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Entities
{
    public class FeedItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int FeedId { get; set; }
        public int SourceId { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }

        public ICollection<FeedLabel> Labels { get; set; }
    }
}
