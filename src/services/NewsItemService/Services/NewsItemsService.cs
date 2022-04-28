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
        private readonly ModelStateDictionary modelState;
        private readonly INewsItemRepository repo;

        public NewsItemsService(IActionContextAccessor modelState, INewsItemRepository repo)
        {
            if(modelState != null)
                this.modelState = modelState.ActionContext.ModelState;
            this.repo = repo;
        }

        public bool CreateNewsItem(CreateNewsItemDTO dto)
        {
            bool success = false; 

            if (dto.Title == null || dto.Title.Trim() == string.Empty)
                modelState.AddModelError("Title", "The title is required.");
            if (dto.UserID <= 0)
                modelState.AddModelError("UserID", "The user ID can't be 0 or smaller");
            if (dto.Content == null || dto.Content.Trim() == string.Empty)
                modelState.AddModelError("Content", "Content is required.");

            if (modelState.IsValid)
            {
                var newsItem = new NewsItem()
                {
                    Content = dto.Content,
                    Title = dto.Title,
                    LocationInformation = dto.LocationInformation
                };
                try
                {
                    repo.CreateNewsItem(newsItem);
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
