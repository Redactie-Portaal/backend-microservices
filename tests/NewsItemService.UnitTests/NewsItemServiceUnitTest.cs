using NewsItemService.DTOs;
using NewsItemService.Services;
using NewsItemService.UnitTests.Stubs;
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
            this._service = new NewsItemOverviewService(new StubNewsItemRepository());
        }

        [Fact]
        public async Task GetSingleNewsItem()
        {
            // Arrange
            List<GetNewsItemDTO> expectedDTOs = new List<GetNewsItemDTO>() {
                new GetNewsItemDTO() { Author = "Robert Bever", NewsItemID = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now }
            };

            // Act
            var newsItems = await this._service.GetNewsItems(1);

            // Assert
            Assert.Equal(expectedDTOs[0].Author, newsItems[0].Author);
            Assert.Equal(expectedDTOs[0].Name, newsItems[0].Name);
        }

        [Fact]
        public async Task GetNoNewsItems()
        {
            // Arrange

            // Act
            var newsItems = await this._service.GetNewsItems(2);

            // Assert
            Assert.Null(newsItems);
        }

        [Fact]
        public async Task GetMultipleNewsItems()
        {
            // Arrange
            List<GetNewsItemDTO> expectedDTOs = new List<GetNewsItemDTO>() {
                new GetNewsItemDTO() { Author = "Harold LööpDeLaInfinite", NewsItemID = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now },
                new GetNewsItemDTO() { Author = "Harold LööpDeLaInfinite", NewsItemID = 2, Name = "Papegaai krijgt medaille.", Status = "Archived", Created = System.DateTime.Now, Updated = System.DateTime.Now }
            };

            // Act
            var newsItems = await this._service.GetNewsItems(5);

            // Assert
            Assert.Equal(expectedDTOs.Count, newsItems.Count);
            Assert.Equal(expectedDTOs[0].Author, newsItems[0].Author);
            Assert.Equal(expectedDTOs[0].Name, newsItems[0].Name);
            Assert.Equal(expectedDTOs[1].Name, newsItems[1].Name);
        }
    }
}