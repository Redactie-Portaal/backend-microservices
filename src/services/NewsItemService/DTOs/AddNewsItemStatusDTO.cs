using NewsItemService.Types;

namespace NewsItemService.DTOs
{
    public class AddNewsItemStatusDTO
    {
        public int NewsItemId { get; set; }
        public NewsItemStatus status { get; set; }
    }
}
