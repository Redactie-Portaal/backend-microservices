using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsItemService.Data;
using NewsItemService.Entities;
using NewsItemService.Tests.IntegrationTests;
using NewsItemService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService.Tests.IntegrationTests
{
    public class NewsItemRepositoryIntegrationTest: IDisposable, IClassFixture<TestConfiguration>
    {
        private readonly NewsItemServiceDatabaseContext _databaseContext;
        private readonly NewsItemRepository _newsItemRepository;
        private readonly TestConfiguration _testConfiguration;
        private readonly IConfiguration _configuration;

        public NewsItemRepositoryIntegrationTest(TestConfiguration testConfiguration)
        {
            this._testConfiguration = testConfiguration;
            this._configuration = _testConfiguration.GetTestDataConfiguration();
            string connectionString = this._configuration.GetConnectionString("LinkContext");

            var serviceProvider = new ServiceCollection().AddEntityFrameworkNpgsql().BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>();
            builder.UseNpgsql(connectionString).UseInternalServiceProvider(serviceProvider);
            this._databaseContext = new NewsItemServiceDatabaseContext(builder.Options);

            this._databaseContext.Database.Migrate();

            this._newsItemRepository = new NewsItemRepository(this._databaseContext);
            SeedData(this._databaseContext);
        }

        /*
        public NewsItemRepositoryIntegrationTest()
        {
            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + "NewsItem").Options;
            var context = new NewsItemServiceDatabaseContext(options);
            SeedProductInMemoryDatabaseWithData(context);

            this._newsItemRepository = new NewsItemRepository(context);
        }
        */

        private void SeedData(NewsItemServiceDatabaseContext context)
        {
            var authors = new List<Author>()
            {
                new Author(){ Id = 1, Name = "TestAuthor", NewsItems = null}
            };

            var newsItems = new List<NewsItem>()
            {
                new NewsItem { Id = 1, Authors = authors, Created = DateTime.Now, Updated = DateTime.Now, Status = NewsItemStatus.Publication},
                new NewsItem { Id = 3, Authors = authors, Created = DateTime.Now, Updated = DateTime.Now, Status = NewsItemStatus.Publication}
            };

            if (!context.NewsItems.Any())
            {
                context.NewsItems.AddRange(newsItems);
            }

            context.SaveChanges();
        }

        #region Get() Tests
        [Fact]
        public void GetSingleItem()
        {
            var result = _newsItemRepository.Get(1);
            Assert.Equal(1, result.Id);
            Assert.Equal("TestAuthor", result.Authors.ElementAt(0).Name);
        }

        [Fact]
        public void GetSingleItemNonexistent()
        {
            var result = _newsItemRepository.Get(2);
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleItemNegative()
        {
            var result = _newsItemRepository.Get(-1);
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleItemZero()
        {
            var result = _newsItemRepository.Get(0);
            Assert.Null(result);
        }

        [Fact]
        public void GetByPage()
        {
            var result = _newsItemRepository.Get(1, 2);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetByNextPage()
        {
            var result = _newsItemRepository.Get(2, 2);
            Assert.Empty(result);
        }
        #endregion

        #region GetByDate() Tests
        [Fact]
        public void GetAfterDate()
        {
            var result = _newsItemRepository.GetAfter(DateTime.Now.AddDays(1), 1, 2);
            Assert.Empty(result);
        }

        [Fact]
        public void GetBeforeDate()
        {
            var result = _newsItemRepository.GetBefore(DateTime.Now.AddDays(-1), 1, 2);
            Assert.Empty(result);
        }

        [Fact]
        public void GetBetweenDate()
        {
            var result = _newsItemRepository.GetBetween(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 1, 2);
            Assert.Equal(2, result.Count);
        }
        #endregion

        [Fact]
        public void AddNewNewsItem()
        {
            Author author = new Author() { Name = "TestAuthor2", NewsItems = null };
            _newsItemRepository.Post(new NewsItem { Authors = new List<Author>(){ author }, Created = DateTime.Now, Updated = DateTime.Now, Status = NewsItemStatus.Production });
            var result = _newsItemRepository.Get(1, 3);
            Assert.Equal(3, result.Count);
        }

        public void Dispose()
        {
            this._databaseContext.Database.EnsureDeleted();
        }
    }
}