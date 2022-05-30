namespace NewsItemService.Entities
{
    public class MediaNewsItem
    {
        public string MediaId { get; set; } = string.Empty;

        public int NewsItemId { get; set; }
        public NewsItem NewsItem { get; set; }

        public bool IsSource { get; set; }
    }
}
