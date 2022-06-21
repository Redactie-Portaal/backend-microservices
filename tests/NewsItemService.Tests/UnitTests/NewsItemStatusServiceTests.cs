using NewsItemService.DTOs;
using NewsItemService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsItemService.Types;
using Xunit;
using NewsItemService.Helpers;
using NewsItemService.Tests.UnitTests.Stubs;

namespace NewsItemService.Tests.UnitTests
{
    /// <summary>
    /// Test for the NewsItemStatusService
    /// </summary>
    public class NewsItemStatusServiceTests
    {
        /// <summary>
        /// NewsItemStatusService for testing purposes
        /// </summary>
        private NewsItemOverviewService _newsItemOverviewService { get; set; }

        /// <summary>
        /// Constructer to build the NewsItemStatusService
        /// </summary>
        public NewsItemStatusServiceTests()
        {
            _newsItemOverviewService = new NewsItemOverviewService(new StubNewsItemRepository(), new StubAuthorRepository());
        }

        /// <summary>
        /// Create a new AddNewsItemStatus object, with parameters
        /// </summary>
        /// <param name="ID">ID of newsItem</param>
        /// <param name="status">Status that the newsItem has to change to</param>
        /// <returns>AddNewsItemStatus</returns>
        private AddNewsItemStatusDTO CreateAddNewsItemStatus(int ID, NewsItemStatus status)
        {
            return new AddNewsItemStatusDTO()
            {
                NewsItemId = ID,
                status = status
            };
        }

        [Fact]
        public async Task Add_EmptyInteger_ReturnsFAULTY_ID()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(default, NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.FAULTY_ID } };
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public async Task Add_EmptyClass_ReturnsDEFAULT()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(null);

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.DEFAULT_OBJECT } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_OutOfRangeEnum_ReturnsINCORRECT_STATUS_VALUE()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(1, (NewsItemStatus)100));

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.INCORRECT_STATUS_VALUE } };
            Assert.Equal(expectedResult, result);
        }


        // Tests for the different enum values status can have
        [Fact]
        public async Task Add_StatusDone_ReturnsStatusChangeToDone()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Done } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusDispose_ReturnsStatusChangeToDispose()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Dispose));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Dispose } };
            Assert.Equal(expectedResult, result);
        }

        //TODO Fix this test once the roles have been implemented
        /*
        [Fact]
        public async Task Add_StatusArchived_ReturnsStatusChangeToArchived()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Archived));
            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Archived } };
            Assert.Equal(expectedResult, result);
        }
        */

        [Fact]
        public async Task Add_StatusProduction_ReturnsStatusChangeToProduction()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Production));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Production } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusPublication_ReturnsStatusChangeToPublication()
        {
            var result = _newsItemOverviewService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Publication));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Publication } };
            Assert.Equal(expectedResult, result);
        }


    }
}