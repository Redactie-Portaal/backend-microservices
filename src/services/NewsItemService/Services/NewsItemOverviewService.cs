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

        public async Task<List<GetNewsItemDTO>?> GetNewsItems(int authorID)
        {
            Author? author = await this._newsItemRepository.GetNewsItems(authorID);
            List<GetNewsItemDTO> newsItemsDTO = new List<GetNewsItemDTO>();

            if (author == null) return null;

            foreach (NewsItem newsItem in author.NewsItems)
            {
                newsItemsDTO.Add(new GetNewsItemDTO() { NewsItemID = newsItem.Id, Name = newsItem.Name, Created = newsItem.Created, Updated = newsItem.Updated, Author = author.Name, Status = newsItem.Status });
            }
            return newsItemsDTO;
        }
    }
}
