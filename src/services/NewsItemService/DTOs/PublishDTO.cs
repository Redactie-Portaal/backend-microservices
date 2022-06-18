using NewsItemService.Entities;

namespace NewsItemService.DTOs
{
    public class PublishDTO
    {
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<MediaDTO> Media { get; set; } = new List<MediaDTO>();
    }
}
