using NewsItemService.DTOs;
using NewsItemService.Services;
using NewsItemService.Types;
using NewsItemService.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService.UnitTests.UnitTests
{
    public class AuthorServiceUnitTest
    {
        private AuthorService _service;

        public AuthorServiceUnitTest()
        {
            this._service = new AuthorService(new StubAuthorRepository());
        }

        #region Get() Tests
        [Fact]
        public void GetAll()
        {
            // Arrange
            var expected = new List<AuthorDTO>()
            {
                new AuthorDTO() { Id = 1, Name = "Jacob" },
                new AuthorDTO() { Id = 2, Name = "Jason" }
            };

            // Act
            var authors = _service.Get();

            // Assert
            Assert.Equal(expected.Count, authors.Count);
        }

        [Fact]
        public void GetSingleAuthor()
        {
            // Arrange
            var expected = new AuthorDTO() { Id = 1, Name = "Jacob" };

            // Act
            var author = _service.Get(1);

            // Assert
            Assert.Equal(expected.Id, author.Id);
            Assert.Equal(expected.Name, author.Name);
        }

        [Fact]
        public void GetSingleAuthorNonexistent()
        {
            // Arrange

            // Act
            var author = _service.Get(3);

            // Assert
            Assert.Null(author);
        }

        [Fact]
        public void GetSingleAuthorNegative()
        {
            // Arrange

            // Act
            var author = _service.Get(-1);

            // Assert
            Assert.Null(author);
        }

        [Fact]
        public void GetSingleAuthorZero()
        {
            // Arrange

            // Act
            var author = _service.Get(0);

            // Assert
            Assert.Null(author);
        }
        #endregion

        #region GetNewsItems() Tests
        [Fact]
        public void GetNewsItems()
        {
            // Arrange
            var expected = new List<NewsItemDTO>()
            {
                new NewsItemDTO()
                {
                    Id = 1,
                    Title = "Title: 1",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO() { Id = 1, Name = "Jacob" },
                        new AuthorDTO() { Id = 2, Name = "Jason" }
                    },
                    Status = NewsItemStatus.Done
                },
                new NewsItemDTO()
                {
                    Id = 2,
                    Title = "Title: 2",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO() { Id = 1, Name = "Jacob" },
                        new AuthorDTO() { Id = 2, Name = "Jason" }
                    },
                    Status = NewsItemStatus.Done
                },
                new NewsItemDTO()
                {
                    Id = 3,
                    Title = "Title: 3",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO() { Id = 1, Name = "Jacob" },
                        new AuthorDTO() { Id = 2, Name = "Jason" }
                    },
                    Status = NewsItemStatus.Done
                },
                new NewsItemDTO()
                {
                    Id = 4,
                    Title = "Title: 4",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO() { Id = 1, Name = "Jacob" },
                        new AuthorDTO() { Id = 2, Name = "Jason" }
                    },
                    Status = NewsItemStatus.Done
                },
                new NewsItemDTO()
                {
                    Id = 5,
                    Title = "Title: 5",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO() { Id = 1, Name = "Jacob" },
                        new AuthorDTO() { Id = 2, Name = "Jason" }
                    },
                    Status = NewsItemStatus.Done
                }
            };

            // Act
            List<NewsItemDTO> newsItems = _service.GetNewsItems(1, 1, 5);

            // Assert
            Assert.Equal(expected.Count, newsItems.Count);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(expected[i].Id, newsItems[i].Id);
                Assert.Equal(expected[i].Title, newsItems[i].Title);
                Assert.Equal(expected[i].Created, newsItems[i].Created);
                Assert.Equal(expected[i].Updated, newsItems[i].Updated);
                Assert.Equal(expected[i].Status, newsItems[i].Status);
            }
        }
        #endregion
    }
}
