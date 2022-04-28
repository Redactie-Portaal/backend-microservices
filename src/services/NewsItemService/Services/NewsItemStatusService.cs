using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Enums;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class NewsItemStatusService
    {

        public Dictionary<bool, string> CheckNewsItemValue(AddNewsItemStatus newsItemStatus)
        {
            if (newsItemStatus.NewsItemId < 1)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.FAULTY_ID" } };
            }
            if (newsItemStatus.NewsItemId.GetType() != typeof(int))
            {
                return new Dictionary<bool, string>() { { false, "STATUS.PARSE_FAULT" } };
            }
            if (!Enum.IsDefined(typeof(NewsItemStatus), newsItemStatus.status))
            {
                return new Dictionary<bool, string>() { { false, "STATUS.INCORRECT_STATUS_VALUE" } };
            }
            return new Dictionary<bool, string>() { { true, "Status able change to " + newsItemStatus.status.ToString() } };
        }

       
    }
}
