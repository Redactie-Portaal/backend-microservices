namespace NewsItemService.DTOs
{
    public class NewsItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<AuthorDTO> Authors { get; set; }
        public string Status { get; set; }
    }
}
