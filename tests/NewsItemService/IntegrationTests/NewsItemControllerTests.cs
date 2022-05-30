using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsArticleService.Controllers;
using NewsItemService.Data;
using NewsItemService.Entities;
using NewsItemService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService
{
    public class NewsItemControllerTests
    {
        private NewsItemController Initialize(bool seed = true, [CallerMemberName] string callerName = "")
        {
            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + callerName).Options;
            var context = new NewsItemServiceDatabaseContext(options);
            if (seed)
            {
                SeedProductInMemoryDatabaseWithData(context);
            }
            var repo = new NewsItemRepository(context);

            return new NewsItemController(repo, new AuthorRepository(context), new CategoryRepository(context));
        }

        private void SeedProductInMemoryDatabaseWithData(NewsItemServiceDatabaseContext context)
        {
            var newsItems = new List<NewsItem>
            {
                new NewsItem { Id = 8, Authors = null, Created = DateTime.Now, Updated = DateTime.Now}
            };

            if (!context.NewsItems.Any())
            {
                context.NewsItems.AddRange(newsItems);
            }
            
            context.SaveChanges();
        }


        [Fact]
        private async Task CreateNewsItemSuccessfully()
        {
            var controller = Initialize();

            List<int> authorIds = new() { 1 };
            var result = controller.Create(new DTOs.CreateNewsItemDTO { AuthorIds = authorIds, Title = "Test title", Content = "Test content" });
            var resulttostring = result.Result as ObjectResult;

            var final = resulttostring.Value.GetType().GetProperty("message").GetValue(resulttostring.Value, null);
            Assert.Equal("Author does not exist", final);
        }
    }
}