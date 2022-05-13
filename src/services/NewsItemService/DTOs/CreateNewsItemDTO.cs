namespace NewsItemService.DTOs
{
    public class CreateNewsItemDTO
    {
        public List<int> AuthorIds { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? LocationDetails { get; set; }
        public string? ContactDetails { get; set; }
        public string? ProductionDate { get; set; }
        public string? Region { get; set; }
        public DateTime CreationDate { get; set; }

        public string? VideoUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public string? ÀudioUrl { get; set; }
    }
}
