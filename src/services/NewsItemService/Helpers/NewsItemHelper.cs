using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Helpers
{
    class NewsItemHelper 
    {
        public static NewsItemDTO ToDTO(NewsItem newsItem, bool includeAuthors = true)
        {
            var newsItemDTO = new NewsItemDTO
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Created = newsItem.Created,
                Updated = newsItem.Updated,
                Status = newsItem.Status
            };

            if (includeAuthors) {
                var authors = new List<AuthorDTO>();

                foreach (var author in newsItem.Authors)
                {
                    authors.Add(AuthorHelper.ToDTO(author));
                }

                newsItemDTO.Authors = authors;
            }

            return newsItemDTO;
        }

        public static List<NewsItemDTO> ToDTO(List<NewsItem> newsItems, bool includeAuthors = true)
        {
            List<NewsItemDTO> newsItemDTOs = new List<NewsItemDTO>();
            foreach (NewsItem newsItem in newsItems)
            {
                newsItemDTOs.Add(NewsItemHelper.ToDTO(newsItem, includeAuthors));
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
                Title = newsItemDTO.Title,
                Created = newsItemDTO.Created,
                Updated = newsItemDTO.Updated,
                Authors = authors,
                Status = newsItemDTO.Status
            };
        }
    }
}
