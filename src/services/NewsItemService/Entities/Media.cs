﻿using NewsItemService.Types;

namespace NewsItemService.Entities
{
    public class Media
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public byte[] FileContent { get; set; }
        public NewsItemMediaType NewsItemMediaType { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}
