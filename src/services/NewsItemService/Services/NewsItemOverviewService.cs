using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;
using NewsItemService.Helpers;

namespace NewsItemService.Services
{
    public class NewsItemOverviewService
    {
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        
        public NewsItemOverviewService(INewsItemRepository newsItemRepository, IAuthorRepository authorRepository)
        {
            this._newsItemRepository = newsItemRepository;
            this._authorRepository = authorRepository;
        }

        public List<NewsItemDTO> Get(int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.Get(page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public NewsItemDTO? Get(int id)
        {
            var newsItem = this._newsItemRepository.Get(id);
            if (newsItem == null) return null;

            return NewsItemHelper.ToDTO(newsItem);
        }

        public List<NewsItemDTO> GetBefore(DateTime date, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetBefore(date.ToUniversalTime(), page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }
        
        public List<NewsItemDTO> GetAfter(DateTime date, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetAfter(date.ToUniversalTime(), page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public List<NewsItemDTO> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetBetween(startDate.ToUniversalTime(), endDate.ToUniversalTime(), page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }
    }
}
