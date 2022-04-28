namespace NewsItemService.DTOs
{
    public class GetNewsItemDTO
    {
        public int NewsItemID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<string> Authors { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
