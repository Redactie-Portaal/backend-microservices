using NewsItemService.DTOs;
using NewsItemService.Services;
using NewsItemService.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService.UnitTests
{
    public class NewsItemServiceUnitTest
    {
        private NewsItemOverviewService _service;

        public NewsItemServiceUnitTest()
        {
            this._service = new NewsItemOverviewService(new StubNewsItemRepository(), new StubAuthorRepository());
        }

        [Fact]
        public void GetSingleNewsItem()
        {
            // Arrange
            var expected = new NewsItemDTO()
            {
                Id = 1,
                Name = "Title: 1",
                Created = new DateTime(2020, 1, 1),
                Updated = new DateTime(2020, 1, 1),
                Authors = new List<AuthorDTO>()
                {
                    new AuthorDTO()
                    {
                        Id = 1,
                        Name = "James"
                    }
                },
                Status = "Processing"
            };

            // Act
            var newsItemDTO = _service.Get(1);

            // Assert
            Assert.Equal(expected.Id, newsItemDTO.Id);
            Assert.Equal(expected.Name, newsItemDTO.Name);
            Assert.Equal(expected.Created, newsItemDTO.Created);
            Assert.Equal(expected.Updated, newsItemDTO.Updated);
            Assert.Equal(expected.Authors[0].Id, newsItemDTO.Authors[0].Id);
            Assert.Equal(expected.Authors[0].Name, newsItemDTO.Authors[0].Name);
            Assert.Equal(expected.Status, newsItemDTO.Status);
        }

        [Fact]
        public void GetUnknownNewsItems()
        {
            // Arrange

            // Act
            NewsItemDTO? newsItems = _service.Get(101);

            // Assert
            Assert.Null(newsItems);
        }

        [Fact]
        public void GetNewsItemWithIDZero()
        {
            // Arrange

            // Act
            NewsItemDTO? newsItems = _service.Get(0);

            // Assert
            Assert.Null(newsItems);
        }

        [Fact]
        public void GetNewsItemWithNegativeID()
        {
            // Arrange

            // Act
            NewsItemDTO? newsItems = _service.Get(-1);

            // Assert
            Assert.Null(newsItems);
        }

        [Fact]
        public void GetPageOneFiveNewsItems()
        {
            // Arrange
            var expected = new List<NewsItemDTO>()
            {
                new NewsItemDTO() {
                    Id = 1,
                    Name = "Title: 1",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            Id = 1,
                            Name = "James"
                        }
                    },
                    Status = "Processing"
                },
                new NewsItemDTO() {
                    Id = 2,
                    Name = "Title: 2",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            Id = 1,
                            Name = "James"
                        }
                    },
                    Status = "Processing"
                },
                 new NewsItemDTO() {
                    Id = 3,
                    Name = "Title: 3",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            Id = 1,
                            Name = "James"
                        }
                    },
                    Status = "Processing"
                },
                 new NewsItemDTO() {
                    Id = 4,
                    Name = "Title: 4",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            Id = 1,
                            Name = "James"
                        }
                    },
                    Status = "Processing"
                },
                 new NewsItemDTO() {
                    Id = 5,
                    Name = "Title: 5",
                    Created = new DateTime(2020, 1, 1),
                    Updated = new DateTime(2020, 1, 1),
                    Authors = new List<AuthorDTO>()
                    {
                        new AuthorDTO()
                        {
                            Id = 1,
                            Name = "James"
                        }
                    },
                    Status = "Processing"
                },
            };

            // Act
            List<NewsItemDTO> newsItems = _service.Get(1, 5);

            // Assert
            Assert.Equal(expected.Count, newsItems.Count);
            for (int i = 0; i < 5; i++) {
                Assert.Equal(expected[i].Id, newsItems[i].Id);
                Assert.Equal(expected[i].Name, newsItems[i].Name);
                Assert.Equal(expected[i].Created, newsItems[i].Created);
                Assert.Equal(expected[i].Updated, newsItems[i].Updated);
                Assert.Equal(expected[i].Authors[0].Id, newsItems[i].Authors[0].Id);
                Assert.Equal(expected[i].Authors[0].Name, newsItems[i].Authors[0].Name);
                Assert.Equal(expected[i].Status, newsItems[i].Status);
            }
        }
    }
}