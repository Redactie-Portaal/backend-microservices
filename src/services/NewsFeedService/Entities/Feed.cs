using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Entities
{
    public class Feed
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeArrival { get; set; }
        public bool IsArchived { get; set; }
        public bool IsReady { get; set; }
        public int ExistingStoryID { get; set; }

    }
}
