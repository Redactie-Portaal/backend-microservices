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
    /// Integration Tests for the Author repository
    /// </summary>
    public class AuthorRepositoryIntegrationTest : IDisposable
    {
        /// <summary>
        /// AuthorRepository for testing purposes
        /// </summary>
        private readonly AuthorRepository _authorRepository;
        private readonly NewsItemServiceDatabaseContext _databaseContext;
        private readonly ILogger<AuthorRepository> _logger;

        /// <summary>
        /// Constructor to setup the in memory database, and add to the context to use.
        /// </summary>
        public AuthorRepositoryIntegrationTest()
        {
            var serviceProvider = new ServiceCollection()
              .AddEntityFrameworkInMemoryDatabase()
              .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb_" + "AuthorOverview")
                .UseInternalServiceProvider(serviceProvider).Options;
            _databaseContext = new NewsItemServiceDatabaseContext(options);
            SeedData(_databaseContext);

            _authorRepository = new AuthorRepository(_databaseContext, _logger);
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
                new NewsItem { Id = 1, Title = "First Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 2, Title = "Second Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 3, Title = "Third Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication},
                new NewsItem { Id = 4, Title = "Fourth Title", Authors = authors, Created = DateTime.Now.ToUniversalTime(), Updated = DateTime.Now.ToUniversalTime(), Status = NewsItemStatus.Publication}
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
        public void GetNewsItemsFromPageOne()
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
            List<NewsItem> newsItems = _authorRepository.GetNewsItems(1, 1, 2);

            // Assert
            Assert.Equal(expected.Count, newsItems.Count);
            for (int i = 0; i < newsItems.Count; i++)
            {
                Assert.Equal(expected[i].Id, newsItems[i].Id);
                Assert.Equal(expected[i].Title, newsItems[i].Title);
                Assert.Equal(expected[i].Status, newsItems[i].Status);
            }
        }

        [Fact]
        public void GetNewsItemsFromPageTwo()
        {
            // Arrange
            var expected = new List<NewsItem>()
            {
                new NewsItem()
                {
                    Id = 3,
                    Title = "Third Title",
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
                    Id = 4,
                    Title = "Fourth Title",
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
            List<NewsItem> newsItems = _authorRepository.GetNewsItems(1, 2, 2);

            // Assert
            Assert.Equal(expected.Count, newsItems.Count);
            for (int i = 0; i < newsItems.Count; i++)
            {
                Assert.Equal(expected[i].Id, newsItems[i].Id);
                Assert.Equal(expected[i].Title, newsItems[i].Title);
                Assert.Equal(expected[i].Status, newsItems[i].Status);
            }
        }

        [Fact]
        public void GetZeroNewsItems()
        {
            // Arrange

            // Act
            List<NewsItem> newsItems = _authorRepository.GetNewsItems(4, 1, 2);

            // Assert
            Assert.Null(newsItems);
        }
        #endregion

        [Fact]
        public void AddNewAuthor()
        {
            // Arrange
            var author = new Author() { Name = "Steven" };

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
