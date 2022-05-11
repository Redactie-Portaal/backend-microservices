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

        public async Task<List<GetNewsItemDTO>?> GetNewsItemsByAuthor(int authorID)
        {
            List<NewsItem>? newsItems = await this._newsItemRepository.GetNewsItems(authorID);

            if (newsItems == null) return null;

            return ConvertToDTOs(newsItems);
        }

        public async Task<List<GetNewsItemDTO>?> GetNewsItemsBeforeDate(DateTime date)
        {
            List<NewsItem>? newsItems = await this._newsItemRepository.GetNewsItemsBeforeDate(date);

            if (newsItems == null) return null;

            return ConvertToDTOs(newsItems);
        }

        private List<GetNewsItemDTO> ConvertToDTOs(List<NewsItem> newsItems)
        {
            List<GetNewsItemDTO> newsItemsDTO = new List<GetNewsItemDTO>();
            foreach (NewsItem newsItem in newsItems)
            {
                var names = new List<string>();
                foreach (Author a in newsItem.Authors)
                {
                    names.Add(a.Name);
                }
                newsItemsDTO.Add(new GetNewsItemDTO() { NewsItemID = newsItem.Id, Name = newsItem.Name, Created = newsItem.Created, Updated = newsItem.Updated, Status = newsItem.Status, Authors = names });
            }
            return newsItemsDTO;
        }
    }
}
