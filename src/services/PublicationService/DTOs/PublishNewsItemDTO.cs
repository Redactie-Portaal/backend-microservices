namespace PublicationService.DTOs
{
    public class PublishNewsItemDTO
    {
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<MediaDTO> Pictures { get; set; } = new List<MediaDTO>();
        public List<MediaDTO> Videos { get; set; } = new List<MediaDTO>();
    }
}
