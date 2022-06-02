namespace PublicationService.DTOs
{
    public class PublishNewsItemDTO
    {
        public string Summary { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> MediaFileNames { get; set; } = new List<string>();
    }
}
