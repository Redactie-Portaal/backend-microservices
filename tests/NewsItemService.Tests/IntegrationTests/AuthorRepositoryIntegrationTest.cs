using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class AuthorRepositoryIntegrationTest : IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _databaseContext;
        private readonly AuthorRepository _authorRepository;
        private readonly ILogger<AuthorRepository> _authorRepositorylogger;

        public AuthorRepositoryIntegrationTest(ILogger<AuthorRepository> authorRepositorylogger)
        {
            string connectionString = "Server=localhost;Port=1111;Database=integrationtests;UserId=developer;Password=developer";
            var serviceProvider = new ServiceCollection().AddEntityFrameworkNpgsql().BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>();
            builder.UseNpgsql(connectionString).UseInternalServiceProvider(serviceProvider);
            this._databaseContext = new NewsItemServiceDatabaseContext(builder.Options);

            this._databaseContext.Database.EnsureCreated();

            SeedData(this._databaseContext);

            _authorRepositorylogger = authorRepositorylogger;
            this._authorRepository = new AuthorRepository(this._databaseContext, _authorRepositorylogger);
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
                new NewsItem { Id = 1, Title = "First Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 2, Title = "Second Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication}
            };

            if (!context.NewsItems.Any())
            {
                context.NewsItems.AddRange(newsItems);
            }

            context.SaveChanges();
        }

        #region Get() Tests
        [Fact]
        public void GetAll()
        {
            // Arrange

            // Act
            var result = _authorRepository.Get();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetSingleAuthor()
        {
            // Arrange

            // Act
            var result = _authorRepository.Get(1);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("TestAuthor", result.Name);
        }

        [Fact]
        public void GetSingleAuthorNonexistent()
        {
            // Arrange

            // Act
            var result = _authorRepository.Get(3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleAuthorNegative()
        {
            // Arrange

            // Act
            var result = _authorRepository.Get(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSingleAuthorZero()
        {
            // Arrange

            // Act
            var result = _authorRepository.Get(0);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region GetNewsItems() Tests
        [Fact]
        public void GetNewsItems()
        {
            // Arrange
            var expected = new List<NewsItem>()
            {
                new NewsItem()
                {
                    Id = 1,
                    Title = "First Title",
                    Created = DateTime.Now.ToUniversalTime(),
                    Updated = DateTime.Now.ToUniversalTime(),
                    Authors = new List<Author>()
                    {
                        new Author(){ Id = 1, Name = "TestAuthor"},
                        new Author(){ Id = 2, Name = "TestAuthor2"}
                    },
                    Status = NewsItemStatus.Publication
                },
                new NewsItem()
                {
                    Id = 2,
                    Title = "Second Title",
                    Created = DateTime.Now.ToUniversalTime(),
                    Updated = DateTime.Now.ToUniversalTime(),
                    Authors = new List<Author>()
                    {
                        new Author(){ Id = 1, Name = "TestAuthor"},
                        new Author(){ Id = 2, Name = "TestAuthor2"}
                    },
                    Status = NewsItemStatus.Publication
                }
            };

            // Act
            List<NewsItem> newsItems = _authorRepository.GetNewsItems(1, 1, 5);

            // Assert
            Assert.Equal(expected.Count, newsItems.Count);
            for (int i = 0; i < newsItems.Count; i++)
            {
                Assert.Equal(expected[i].Id, newsItems[i].Id);
                Assert.Equal(expected[i].Title, newsItems[i].Title);
                Assert.Equal(expected[i].Status, newsItems[i].Status);
            }
        }
        #endregion

        [Fact]
        public void AddNewAuthor()
        {
            // Arrange
            var author = new Author() { Id = 3, Name = "Steven" };

            // Act
            _authorRepository.Post(author);
            var result = _authorRepository.Get();

            // Assert
            Assert.Equal(3, result.Count);
        }

        public void Dispose()
        {
            this._databaseContext.Database.EnsureDeleted();
        }
    }
}
