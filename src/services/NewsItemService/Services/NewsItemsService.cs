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
        private readonly ICategoryRepository categoryRepository;

        public NewsItemsService(INewsItemRepository repo, ICategoryRepository categoryRepository)
        {
            newsitemRepository = repo;
            categoryRepository = categoryRepository;
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
                Title = dto.Title,
                Authors = authors,
                LocationInformation = dto.LocationInformation,
                ContactInformation = dto.ContactInformation,
                Region = dto.Region,
                Created = dto.ProductionDate.ToUniversalTime()
            };

            if (dto.CategoryIds.Count != 0)
            {
                //newsItem.Categories = new List<Category>();

                foreach (var id in dto.CategoryIds)
                {
                    var category = await categoryRepository.GetCategoryById(id);
                    if (category.FirstOrDefault().Key == false)
                    {
                        return new Dictionary<bool, string>() { { false, "Category does not exist" } };
                    }
                    authors.Add(category.FirstOrDefault().Value);
                }

            }
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
