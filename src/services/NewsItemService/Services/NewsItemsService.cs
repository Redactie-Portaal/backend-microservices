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

        //public NewsItemsService(IActionContextAccessor modelState, INewsItemRepository repo)
        //{
        //    if(modelState != null)
        //        this.modelState = modelState.ActionContext.ModelState;
        //    this.repo = repo;
        //}

        public NewsItemsService(INewsItemRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> CreateNewsItem(CreateNewsItemDTO dto)
        {
            bool success = false; 

            if (dto.Title == null || dto.Title.Trim() == string.Empty)
                modelState.AddModelError("Title", "The title is required.");
            if (dto.AuthorIds.Count == 0)
                modelState.AddModelError("UserID", "The user ID can't be 0 or smaller");
            if (dto.Content == null || dto.Content.Trim() == string.Empty)
                modelState.AddModelError("Content", "Content is required.");

            //Moeten nog wat meer checks bij en de ModelState misschien anders om het op dezelfde manier te doen als de rest?

            if (modelState.IsValid)
            {
                List<Author> authors = new();
                foreach (var id in dto.AuthorIds)
                {
                    var author = await repo.GetAuthorById(id);
                    authors.Add(new Author()
                    {
                        Id = author.FirstOrDefault().Value.Id,
                        Name = author.FirstOrDefault().Value.Name
                    });
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
                    await repo.CreateNewsItem(newsItem);
                    success = true;
                }
                catch
                {
                   //Something
                }
            }    
            else
                 success = false;

            return success;
                
        }
    }
}
