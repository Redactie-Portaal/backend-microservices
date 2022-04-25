namespace NewsItemService.DTOs
{
    public class GetNewsItemDTO
    {
        public enum status { Discard, Production, Done, Publication, Archived}
        public int NewsItemID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<string> Authors { get; set; } = new List<string>();
        public status Status { get; set; }
    }
}
