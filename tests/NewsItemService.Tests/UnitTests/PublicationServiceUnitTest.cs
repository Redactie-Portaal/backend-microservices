using NewsItemService.Entities;
using NewsItemService.Services;
using NewsItemService.Tests.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService.Tests.UnitTests
{
    public class PublicationServiceUnitTest
    {
        private PublicationService _service;

        public PublicationServiceUnitTest()
        {
            this._service = new PublicationService(new StubNewsItemRepository(), new StubPublicationRepository(), new StubMediaNewsItemRepository(), new StubMessageProducer());
        }

        #region GetById() tests
        [Fact]
        public void GetSinglePublication()
        {
            // Arrange
            Publication expected = new Publication()
            {
                Id = 1,
                Name = "Twitter",
                Description = "Social media",
                Icon = "empty.png"
            };

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Equal(expected.Id, result.Result.SingleOrDefault().Value.Id);
            Assert.Equal(expected.Name, result.Result.SingleOrDefault().Value.Name);
        }

        [Fact]
        public void GetPublictionReturnNull()
        {
            // Arrange

            // Act
            var result = _service.GetById(2);

            // Assert
            Assert.Null(result.Result.SingleOrDefault().Value);
        }

        [Fact]
        public void GetPublicationNegative()
        {
            // Act
            var result = _service.GetById(-1);

            // Assert
            Assert.Null(result.Result.SingleOrDefault().Value);
        }
        #endregion

        [Fact]
        public void PublishNewsItemGetNonexistentNewsItemId()
        {
            // Arrange

            // Act
            var result = _service.PublishNewsItem(999, 1);

            // Assert
            Assert.False(result.Result.SingleOrDefault().Key);
        }
    }
}
