using NewsItemService.Types;

namespace NewsItemService.Entities
{
    public class Media
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public NewsItemMediaType NewsItemMediaType { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
