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
        private readonly INewsItemRepository repo;

        public NewsItemsService(INewsItemRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Dictionary<bool, string>> CreateNewsItem(CreateNewsItemDTO dto)
        {
            //TODO: add the checks for variable values to the controller
            #region add these checks to the controller
            if (dto.Title == null || dto.Title.Trim() == string.Empty)
                modelState.AddModelError("Title", "The title is required.");
            if (dto.AuthorIds.Count == 0)
                modelState.AddModelError("UserID", "The user ID can't be 0 or smaller");
            if (dto.Content == null || dto.Content.Trim() == string.Empty)
                modelState.AddModelError("Content", "Content is required.");
            #endregion

            //Moeten nog wat meer checks bij en de ModelState misschien anders om het op dezelfde manier te doen als de rest?
            List<Author> authors = new();
            foreach (var id in dto.AuthorIds)
            {
                var author = await repo.GetAuthorById(id);
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
                LocationDetails = dto.LocationDetails,
                ContactDetails = dto.ContactDetails,
                Region = dto.Region,
                Created = dto.CreationDate


            };
            try
            {
                return await repo.CreateNewsItem(newsItem);
            }
            catch (Exception)
            {
                throw; //Something
            }
        }
    }
}
