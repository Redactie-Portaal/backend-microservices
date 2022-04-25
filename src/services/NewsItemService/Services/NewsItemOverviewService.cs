using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class NewsItemOverviewService
    {
        private readonly INewsItemRepository _newsItemRepository;
        

        public NewsItemOverviewService(INewsItemRepository newsItemRepository)
        {
            this._newsItemRepository = newsItemRepository;
        }

        public List<GetNewsItemDTO> GetNewsItems(int authorID)
        {
            List<NewsItem> newsItems = this._newsItemRepository.GetNewsItems(authorID);
            List<GetNewsItemDTO> newsItemsDTO = new List<GetNewsItemDTO>();

            foreach (NewsItem newsItem in newsItems)
            {
                newsItemsDTO.Add(new GetNewsItemDTO() { NewsItemID = newsItem.Id, Created = newsItem.Created, Updated = newsItem.Updated,  });
            }
            return newsItemsDTO;
        }
    }
}
