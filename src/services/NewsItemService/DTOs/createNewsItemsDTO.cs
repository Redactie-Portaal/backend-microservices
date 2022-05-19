using NewsItemService.Types;

namespace NewsItemService.DTOs
{
    public class createNewsItemDTO
    {
        public string Title { get; set; }
        public List<int>? AuthorIDs { get; set; }
        public List<int>? CategoryIDs { get; set; }
        public List<int>? PublicationIDs { get; set; }
        public List<int>? TagIDs { get; set; }
        public NewsItemStatus Status { get; set; }
        public DateTime EndDate { get; set; }
        public bool ReadyToCheck { get; set; } = false;
        public string Content { get; set; }
        public string? ContactInformation { get; set; }
        public string? LocationInformation { get; set; }
        public string? InfoFeed { get; set; }
        public string? Region { get; set; }
        public string? SourceData { get; set; }
        public string? Copyright { get; set; }
        public string? AudioUrl { get; set; }
        public string? PictureUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? DocumentUrl { get; set; }
        public string? SocialMediaUrl { get; set; }
    }
}
