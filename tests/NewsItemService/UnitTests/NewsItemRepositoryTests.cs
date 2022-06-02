using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService.Tests.UnitTests
{
    /// <summary>
    /// Tests for the item repository
    /// </summary>
    public class NewsItemRepositoryTests
    {
        /// <summary>
        /// NewsItemRepository for testing purposes
        /// </summary>
        private NewsItemRepository repo { get; set; }
        private readonly NewsItemServiceDatabaseContext context;
        private readonly ILogger<NewsItemRepository> _logger;

        /// <summary>
        /// Constructor to setup the in memory database, and add to the context to use.
        /// </summary>
        public NewsItemRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + "NewsItem").Options;
            context = new NewsItemServiceDatabaseContext(options);
            //SeedProductInMemoryDatabaseWithData(context);
            this.repo = new NewsItemRepository(context, _logger);
        }



        /// <summary>
        /// Feed the virtual database data
        /// </summary>
        /// <param name="context">Context used for the repository</param>
        //private void SeedProductInMemoryDatabaseWithData(NewsItemServiceDatabaseContext context)
        //{
        //    var authors = new List<Author>()
        //    {
        //        new Author(){ Id = 1, Name = "TestAuthor", NewsItems = null}
        //    };

        //    var newsItems = new List<NewsItem>
        //    {
        //        new NewsItem { Id = 2, Authors = authors, Created = DateTime.Now, Updated = DateTime.Now },
        //        new NewsItem { Id = 3, Authors = authors, Created = DateTime.Now, Updated = DateTime.Now }
        //    };

        //    if (!context.NewsItems.Any())
        //    {
        //        context.NewsItems.AddRange(newsItems);
        //    }

        //    context.SaveChanges();
        //}

        [Fact]
        private async Task CreateNewsItemSuccessfully()
        {
            var authors = new List<Author>()
            {
                new Author(){ Id = 5, Name = "TestAuthor", NewsItems = null}
            };

            NewsItem item = new()
            {
                Id = 66,
                Authors = authors,
                Created = DateTime.Now,
                Title = "test title"

            };

            var result = await repo.CreateNewsItem(item);
            var createdNewsItem = context.NewsItems.Find(66);


            Assert.Equal(result, new Dictionary<bool, string>() { { true, $"Article '{item.Title}' has been created succesfully" } });
            Assert.NotNull(createdNewsItem);

        }

        [Fact]
        private async Task CreateNewsItemFailBecauseNoAuthors()
        {
            var authors = new List<Author>()
            {
             
            };

            NewsItem itemDupe = new()
            {
                Id = 68,
                Authors = authors,
                Created = DateTime.Now,
                Title = "test title"

            };

            context.NewsItems.Add(itemDupe);
            context.SaveChanges();

            NewsItem item = new()
            {
                Id = 67,
                Authors = authors,
                Created = DateTime.Now,
                Title = "test title"

            };

            var result = await repo.CreateNewsItem(item);


            Assert.Equal(result, new Dictionary<bool, string>() { { false, $"Can't create newsItem with a title that has already been used" } });

        }
    }
}
