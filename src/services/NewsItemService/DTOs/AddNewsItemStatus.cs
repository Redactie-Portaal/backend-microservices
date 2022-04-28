using NewsItemService.Enums;

namespace NewsItemService.DTOs
{
    public class AddNewsItemStatus
    {
        public int NewsItemId { get; set; }
        public NewsItemStatus status { get; set; }
    }
}
