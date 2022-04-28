namespace NewsItemService.DTOs
{
    public class GetNewsItemDTO
    {
        public int NewsItemID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
