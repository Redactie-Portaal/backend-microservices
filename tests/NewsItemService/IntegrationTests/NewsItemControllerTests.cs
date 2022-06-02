using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NewsItemRepository> _newsItemLogger;
        private readonly ILogger<AuthorRepository> _authorLogger;
        private readonly ILogger<CategoryRepository> _categoryLogger;

        private readonly ILogger<PublicationRepository> _publicationLogger;
        private readonly ILogger<TagRepository> _tagLogger;
        private readonly ILogger<MediaRepository> _mediaLogger;
        private readonly ILogger<MediaNewsItemRepository> _mediaNewsItemLogger;
        private readonly ILogger<SourceLocationRepository> _sourceLocationLogger;
        private readonly ILogger<SourcePersonRepository> _sourcePersonLogger;
        private readonly ILogger<NoteRepository> _noteLogger;

        private NewsItemController Initialize(bool seed = true, [CallerMemberName] string callerName = "")
        {
            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + callerName).Options;
            var context = new NewsItemServiceDatabaseContext(options);
            if (seed)
            {
                SeedProductInMemoryDatabaseWithData(context);
            }
            var newsItemRepo = new NewsItemRepository(context, _newsItemLogger);

            return new NewsItemController(newsItemRepo, 
                new AuthorRepository(context, _authorLogger), 
                new CategoryRepository(context, _categoryLogger),
                new PublicationRepository(context, _publicationLogger),
                new TagRepository(context, _tagLogger),
                new MediaRepository(_mediaLogger),
                new MediaNewsItemRepository(context, _mediaNewsItemLogger),
                new SourceLocationRepository(context, _sourceLocationLogger),
                new SourcePersonRepository(context, _sourcePersonLogger),
                new NoteRepository(context, _noteLogger));
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