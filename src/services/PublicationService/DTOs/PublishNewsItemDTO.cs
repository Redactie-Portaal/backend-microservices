namespace PublicationService.DTOs
{
    public class PublishNewsItemDTO
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new List<string>();
        public string Content { get; set; } = string.Empty;
        public List<string> MediaFileNames { get; set; } = new List<string>();
    }
}
