﻿using NewsItemService.Entities;

namespace NewsItemService.DTOs
{
    public class PublishDTO
    {
        public string Summary { get; set; }
        public string Content { get; set; }
        public List<Tag> Tags { get; set; }
        public List<MediaDTO> Pictures { get; set; }
        public List<MediaDTO> Videos { get; set; }
    }
}
