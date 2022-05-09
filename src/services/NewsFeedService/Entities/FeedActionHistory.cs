﻿using System.ComponentModel.DataAnnotations;

namespace NewsFeedService.Entities
{
    public class FeedActionHistory
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Action { get; set; }
    }
}
