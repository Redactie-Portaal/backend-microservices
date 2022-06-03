
using NewsItemService.Types;
using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class NewsItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<Author> Authors { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Publication> Publications { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public NewsItemStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime EndDate { get; set; }
        public bool ReadyToCheck { get; set; }
        public string Content { get; set; } = string.Empty;
        public string InfoFeed { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string SocialMediaUrl { get; set; } = string.Empty;
        public ICollection<MediaNewsItem> MediaNewsItems { get; set; }

        public ICollection<SourceLocation> SourceLocations { get; set; }
        public ICollection<SourcePerson> SourcePeople { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
