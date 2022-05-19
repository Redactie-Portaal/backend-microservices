using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Helpers
{
    class NewsItemHelper 
    {
        public static NewsItemDTO ToDTO(NewsItem newsItem)
        {
            var authors = new List<AuthorDTO>();
            foreach (var author in newsItem.Authors)
            {
                authors.Add(AuthorHelper.ToDTO(author));
            }

            return new NewsItemDTO
            {
                Id = newsItem.Id,
                Name = newsItem.Name,
                Created = newsItem.Created,
                Updated = newsItem.Updated,
                Authors = authors,
                Status = newsItem.Status
            };
        }

        public static List<NewsItemDTO> ToDTO(List<NewsItem> newsItems)
        {
            List<NewsItemDTO> newsItemDTOs = new List<NewsItemDTO>();
            foreach (NewsItem newsItem in newsItems)
            {
                newsItemDTOs.Add(NewsItemHelper.ToDTO(newsItem));
            }

            return newsItemDTOs;
        }

        public static NewsItem ToNewsItem(NewsItemDTO newsItemDTO)
        {
            var authors = new List<Author>();
            foreach (var author in newsItemDTO.Authors)
            {
                authors.Add(AuthorHelper.ToAuthor(author));
            }

            return new NewsItem
            {
                Id = newsItemDTO.Id,
                Name = newsItemDTO.Name,
                Created = newsItemDTO.Created,
                Updated = newsItemDTO.Updated,
                Authors = authors,
                Status = newsItemDTO.Status
            };
        }
    }
}
