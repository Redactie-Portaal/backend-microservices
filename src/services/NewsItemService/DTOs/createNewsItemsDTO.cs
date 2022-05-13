namespace NewsItemService.DTOs
{
    public class createNewsItemDTO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public List<int> AuthorIDs { get; set; }
    }
}
