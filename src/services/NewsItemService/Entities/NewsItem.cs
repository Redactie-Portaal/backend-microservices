
﻿using NewsItemService.Types;
using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class NewsItem
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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

        public string ContactInformation { get; set; } = string.Empty;
        public string LocationInformation { get; set; } = string.Empty;
        public string InfoFeed { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string SourceData { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string AudioUrl { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public string SocialMediaUrl { get; set; } = string.Empty;
    }
}
