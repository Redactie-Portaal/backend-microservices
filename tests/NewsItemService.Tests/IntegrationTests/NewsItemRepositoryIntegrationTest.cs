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
    public class NewsItemRepositoryIntegrationTest: IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _databaseContext;
        private readonly NewsItemRepository _newsItemRepository;

        public NewsItemRepositoryIntegrationTest()
        {
            string connectionString = "Server=localhost;Port=1111;Database=integrationtests;UserId=developer;Password=developer";
            var serviceProvider = new ServiceCollection().AddEntityFrameworkNpgsql().BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>();
            builder.UseNpgsql(connectionString).UseInternalServiceProvider(serviceProvider);
            this._databaseContext = new NewsItemServiceDatabaseContext(builder.Options);

            this._databaseContext.Database.EnsureCreated();

            SeedData(this._databaseContext);

            this._newsItemRepository = new NewsItemRepository(this._databaseContext);
        }

        private void SeedData(NewsItemServiceDatabaseContext context)
        {
            var authors = new List<Author>()
            {
                new Author(){ Id = 1, Name = "TestAuthor", NewsItems = null},
                new Author(){ Id = 2, Name = "TestAuthor2", NewsItems = null}
            };

            var newsItems = new List<NewsItem>()
            {
                new NewsItem { Id = 1, Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 2, Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication}
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
            var result = _newsItemRepository.GetAfter(DateTime.Now.AddDays(1).ToUniversalTime(), 1, 2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetBeforeDate()
        {
            // Arrange

            // Act
            var result = _newsItemRepository.GetBefore(DateTime.Now.AddDays(-1).ToUniversalTime(), 1, 2);

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
        #endregion

        [Fact]
        public void AddNewNewsItem()
        {
            // Arrange
            var newsItem = new NewsItem() { Id = 3, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Production };

            var author = _databaseContext.Authors.Include("NewsItems").FirstOrDefault(a => a.Id == 1);
            newsItem.Authors = new List<Author>();
            newsItem.Authors.Add(author);

            // Act
            _newsItemRepository.Post(newsItem);
            var result = _newsItemRepository.Get(1, 3);

            // Assert
            Assert.Equal(3, result.Count);
        }

        public void Dispose()
        {
            this._databaseContext.Database.EnsureDeleted();
        }
    }
}