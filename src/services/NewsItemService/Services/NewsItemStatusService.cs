using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;
using NewsItemService.Types;

namespace NewsItemService.Services
{
    public class NewsItemStatusService
    {
        public NewsItemDisposedDTO NewsItemToDisposedDTO(NewsItem newsItem)
        {
            if (newsItem == default)
            {
                throw new ArgumentNullException(nameof(newsItem));
            }
            List<int> authorIds = new List<int>();
            foreach (var item in newsItem.Authors)
            {
                authorIds.Add(item.Id);
            }

            return new NewsItemDisposedDTO()
            {
                Id = newsItem.Id,
                AuthorIds = authorIds,
                Created = newsItem.Created,
                Status = newsItem.Status,
                Updated = newsItem.Updated
            };
        }
        public Dictionary<bool, string> CheckNewsItemValue(AddNewsItemStatusDTO newsItemStatus)
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
