using NewsItemService.Types;

namespace NewsItemService.DTOs
{
    public class NewsItemDisposedDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<int> AuthorIds { get; set; }
        public NewsItemStatus Status { get; set; }
    }
}
