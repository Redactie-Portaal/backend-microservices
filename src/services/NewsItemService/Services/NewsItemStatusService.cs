using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Enums;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    //TODO: remove this code that belongs to another branch
    public class NewsItemStatusService
    {
        public Dictionary<bool, string> CheckNewsItemValue(AddNewsItemStatus newsItemStatus)
        {
            if (newsItemStatus == default)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.DEFAULT_OBJECT" } };
            }

            if (newsItemStatus.NewsItemId < 0 || newsItemStatus.NewsItemId == default)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.FAULTY_ID" } };
            }

            if (!Enum.IsDefined(typeof(NewsItemStatus), newsItemStatus.status))
            {
                return new Dictionary<bool, string>() { { false, "STATUS.INCORRECT_STATUS_VALUE" } };
            }

            // TODO Add function that checks for roles for user so that the status may not be changed to archived by everyone
            if (newsItemStatus.status == NewsItemStatus.Archived /* && add role check*/)
            {
                return new Dictionary<bool, string>() { { false, "STATUS.INSUFFICIENT_PERMISSIONS" } };
            }
            return new Dictionary<bool, string>() { { true, "Status able change to " + newsItemStatus.status.ToString() } };
        }

       
    }
}
