using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;
using NewsItemService.Helpers;
using NewsItemService.Types;

namespace NewsItemService.Services
{
    public class NewsItemOverviewService
    {
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        
        public NewsItemOverviewService(INewsItemRepository newsItemRepository, 
                                       IAuthorRepository authorRepository)
        {
            _newsItemRepository = newsItemRepository;
            _authorRepository = authorRepository;
        }

        public List<NewsItemDTO> Get(int page, int pageSize)
        {
            var newsItems = _newsItemRepository.Get(page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public NewsItemDTO? Get(int id)
        {
            var newsItem = _newsItemRepository.Get(id);
            if (newsItem == null) return null;

            return NewsItemHelper.ToDTO(newsItem);
        }

        public List<NewsItemDTO> GetBefore(DateTime date, int page, int pageSize)
        {
            var newsItems = _newsItemRepository.GetBefore(date.ToUniversalTime(), page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }
        
        public List<NewsItemDTO> GetAfter(DateTime date, int page, int pageSize)
        {
            var newsItems = _newsItemRepository.GetAfter(date.ToUniversalTime(), page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public List<NewsItemDTO> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var newsItems = _newsItemRepository.GetBetween(startDate.ToUniversalTime(), 
                                                           endDate.ToUniversalTime(), 
                                                           page, 
                                                           pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public Dictionary<bool, string> CheckNewsItemValue(AddNewsItemStatusDTO newsItemStatus)
        {
            if (newsItemStatus == default)
            {
                return new Dictionary<bool, string>() { { false, NewsItemStatusValues.DEFAULT_OBJECT } };
            }

            if (newsItemStatus.NewsItemId < 0 || newsItemStatus.NewsItemId == default)
            {
                return new Dictionary<bool, string>() { { false, NewsItemStatusValues.FAULTY_ID } };
            }

            if (!Enum.IsDefined(typeof(NewsItemStatus), newsItemStatus.status))
            {
                return new Dictionary<bool, string>() { { false, NewsItemStatusValues.INCORRECT_STATUS_VALUE } };
            }

            // TODO Add function that checks for roles for user so that the status may not be changed to archived by everyone
            if (newsItemStatus.status == NewsItemStatus.Archived /* && add role check*/)
            {
                return new Dictionary<bool, string>() { { false, NewsItemStatusValues.INSUFFICIENT_PERMISSIONS } };
            }
            return new Dictionary<bool, string>() { { true, "Status able change to " + newsItemStatus.status.ToString() } };
        }
    }
}
