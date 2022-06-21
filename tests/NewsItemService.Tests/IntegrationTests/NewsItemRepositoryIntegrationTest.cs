using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
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
    /// <summary>
    /// Integration Tests for the newsitem repository
    /// </summary>
    public class NewsItemRepositoryIntegrationTest: IDisposable
    {
        /// <summary>
        /// NewsItemRepository for testing purposes
        /// </summary>
        private readonly NewsItemRepository _newsItemRepository;
        private readonly NewsItemServiceDatabaseContext _databaseContext;
        private readonly ILogger<NewsItemRepository> _logger;

        /// <summary>
        /// Constructor to setup the in memory database, and add to the context to use.
        /// </summary>
        public NewsItemRepositoryIntegrationTest()
        {
            var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb_" + "NewsItemOverview")
                .UseInternalServiceProvider(serviceProvider).Options;
            _databaseContext = new NewsItemServiceDatabaseContext(options);
            SeedData(_databaseContext);

            _newsItemRepository = new NewsItemRepository(_databaseContext, _logger);
        }

        /// <summary>
        /// Feed the virtual database data
        /// </summary>
        /// <param name="context">Context used for the repository</param>
        private void SeedData(NewsItemServiceDatabaseContext context)
        {
            var authors = new List<Author>()
            {
                new Author(){ Id = 1, Name = "TestAuthor", NewsItems = null},
                new Author(){ Id = 2, Name = "TestAuthor2", NewsItems = null}
            };

            var newsItems = new List<NewsItem>()
            {
                new NewsItem { Id = 1, Title = "People are buying gloves in drones", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 2, Title = "Shipping containers cannot move without fuel", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication}
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
            // Arrange

            // Act
            var result = _newsItemRepository.Get(1);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("TestAuthor", result.Authors.ElementAt(0).Name);
        }

        [Fact]
        public void GetSingleItemNonexistent()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.Get(5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleItemNegative()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.Get(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleItemZero()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.Get(0);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetByPage()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.Get(1, 2);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetByNextPage()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.Get(2, 2);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        #region GetByDate() Tests
        [Fact]
        public void GetAfterDate()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetAfter(new DateTime(2022, 01, 01).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAfterDateReturnZero()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetAfter(new DateTime(2050, 7, 7).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetBeforeDate()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetBefore(new DateTime(2050, 7, 7).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetBeforeDateReturnZero()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetBefore(new DateTime(2014, 7, 16).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetBetweenDate()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetBetween(DateTime.Now.AddDays(-1).ToUniversalTime(), DateTime.Now.AddDays(1).ToUniversalTime(), 1, 2);
            
            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetBetweenDateReturnZero()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetBetween(DateTime.Now.AddYears(20).ToUniversalTime(), DateTime.Now.AddYears(25).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Empty(result);
        }
        #endregion

        //TODO: creating a news item requires more values than this test has prepared.

        #region CreateNewsItem() Tests
        [Fact]
        public void AddNewNewsItem()
        {
            // Arrange
            var newsItem = new NewsItem() { Id = 3, Title = "Lawn mover stuck in tree", Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Production };

            var author = _databaseContext.Authors.Include("NewsItems").FirstOrDefault(a => a.Id == 1);
            newsItem.Authors = new List<Author>();
            newsItem.Authors.Add(author);

            // Act
            _newsItemRepository.CreateNewsItem(newsItem);
            var result = _newsItemRepository.Get(1, 3);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void AddNewNewsItemWithExistingTitle()
        {
            // Arrange
            var newsItem = new NewsItem() { Id = 3, Title = "People are buying gloves in drones", Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Production };

            var author = _databaseContext.Authors.Include("NewsItems").FirstOrDefault(a => a.Id == 1);
            newsItem.Authors = new List<Author>();
            newsItem.Authors.Add(author);

            // Act
            var result = _newsItemRepository.CreateNewsItem(newsItem);

            // Assert
            Assert.False(result.Result.SingleOrDefault().Key);
            Assert.Equal("Can't create newsItem with a title that has already been used", result.Result.SingleOrDefault().Value);
        }
        #endregion

        public void Dispose()
        {
            this._databaseContext.Database.EnsureDeleted();
        }
    }
}