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
                new GetNewsItemDTO() { Authors = new List<string>() { "Robert Bever" }, NewsItemID = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now }
            };

            // Act
            var newsItems = await this._service.GetNewsItems(1);

            // Assert
            Assert.Equal(expectedDTOs[0].Authors[0], newsItems[0].Authors[0]);
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
                new GetNewsItemDTO() { Authors = new List<string>() { "Harold LoopDeLaInfinite" }, NewsItemID = 1, Name = "Vogel valt uit nest.", Status = "Processing", Created = System.DateTime.Now, Updated = System.DateTime.Now },
                new GetNewsItemDTO() { Authors = new List<string>() { "Robert Bever", "Harold LoopDeLaInfinite" }, NewsItemID = 2, Name = "Papegaai krijgt medaille.", Status = "Archived", Created = System.DateTime.Now, Updated = System.DateTime.Now }
            };

            // Act
            var newsItems = await this._service.GetNewsItems(5);

            // Assert
            Assert.Equal(expectedDTOs.Count, newsItems.Count);
            Assert.Equal(expectedDTOs[0].Authors[0], newsItems[0].Authors[0]);
            Assert.Equal(expectedDTOs[0].Name, newsItems[0].Name);
            Assert.Equal(expectedDTOs[1].Name, newsItems[1].Name);
            Assert.Equal(expectedDTOs[1].Authors[0], newsItems[1].Authors[0]);
            Assert.Equal(expectedDTOs[1].Authors[1], newsItems[1].Authors[1]);
        }
    }
}