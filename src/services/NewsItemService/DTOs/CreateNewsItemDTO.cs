namespace NewsItemService.DTOs
{
    public class CreateNewsItemDTO
    {
        public int UserID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? LocationInformation { get; set; }
    }
}
