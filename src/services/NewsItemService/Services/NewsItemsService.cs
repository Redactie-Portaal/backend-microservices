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
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;

        public NewsItemsService(INewsItemRepository repo, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
        {
            _newsItemRepository = repo;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Dictionary<bool, string>> CreateNewsItem(CreateNewsItemDTO dto)
        {
            List<Author> authors = new();
            foreach (var id in dto.AuthorIds)
            {
                var author = await _authorRepository.GetAuthorById(id);
                if (author.FirstOrDefault().Key == false)
                {
                    return new Dictionary<bool, string>() { { false, "Author does not exist" } };
                }
                authors.Add(author.FirstOrDefault().Value);
            }

            List<Category> categories = new();
            if (dto.CategoryIds.Count != 0)
            {
                foreach (var id in dto.CategoryIds)
                {
                    var category = await _categoryRepository.GetCategoryById(id);
                    if (category.FirstOrDefault().Key == false)
                    {
                        return new Dictionary<bool, string>() { { false, "Category does not exist" } };
                    }
                    categories.Add(category.FirstOrDefault().Value);
                }
            }

            var newsItem = new NewsItem()
            {
                Content = dto.Content,
                Title = dto.Title,
                Authors = authors,
                LocationInformation = dto.LocationInformation,
                ContactInformation = dto.ContactInformation,
                Region = dto.Region,
                Created = dto.ProductionDate.ToUniversalTime(),
                Categories = categories
            };
            try
            {
                return await _newsItemRepository.CreateNewsItem(newsItem);
            }
            catch (Exception)
            {
                throw; //Something
            }
        }
    }
}
