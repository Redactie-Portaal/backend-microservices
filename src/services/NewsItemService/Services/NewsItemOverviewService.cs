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

        public NewsItemDTO Post(createNewsItemDTO newsItemDTO) 
        {
            var newsItem = new NewsItem()
            {
                Name = newsItemDTO.Name,
                Status = newsItemDTO.Status,
                Created = DateTime.Now.ToUniversalTime()
            };

            newsItem.Authors = new List<Author>();
            foreach (var id in newsItemDTO.AuthorIDs)
            {
                var author = _authorRepository.Get(id);

                newsItem.Authors.Add(author);
            }

            _newsItemRepository.Post(newsItem);

            return NewsItemHelper.ToDTO(newsItem);
        }

        public List<NewsItemDTO> GetBefore(DateTime date, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetBefore(date, page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }
        
        public List<NewsItemDTO> GetAfter(DateTime date, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetAfter(date, page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }

        public List<NewsItemDTO> GetBetween(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            var newsItems = this._newsItemRepository.GetBetween(startDate, endDate, page, pageSize);

            return NewsItemHelper.ToDTO(newsItems);
        }
    }
}
