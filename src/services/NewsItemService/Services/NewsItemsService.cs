using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class NewsItemsService
    {
        private readonly ModelStateDictionary modelState = new ModelStateDictionary();
        private readonly INewsItemRepository newsitemRepository;

        public NewsItemsService(INewsItemRepository repo)
        {
            newsitemRepository = repo;
        }

        public async Task<Dictionary<bool, string>> CreateNewsItem(CreateNewsItemDTO dto)
        {
            List<Author> authors = new();
            foreach (var id in dto.AuthorIds)
            {
                var author = await newsitemRepository.GetAuthorById(id);
                if (author.FirstOrDefault().Key == false)
                {
                    return new Dictionary<bool, string>() { { false, "Author does not exist" } };
                }
                authors.Add(author.FirstOrDefault().Value);
            }

            var newsItem = new NewsItem()
            {
                Content = dto.Content,
                Name = dto.Name,
                Authors = authors,
                LocationInformation = dto.LocationInformation,
                ContactInformation = dto.ContactInformation,
                Region = dto.Region,
                Created = dto.CreationDate.ToUniversalTime()
            };
            try
            {
                return await newsitemRepository.CreateNewsItem(newsItem);
            }
            catch (Exception)
            {
                throw; //Something
            }
        }
    }
}
