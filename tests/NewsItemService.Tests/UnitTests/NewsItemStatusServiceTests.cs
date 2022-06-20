using NewsItemService.DTOs;
using NewsItemService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsItemService.Types;
using Xunit;

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
        private NewsItemStatusService NewsItemStatusService { get; set; }

        /// <summary>
        /// Constructer to build the NewsItemStatusService
        /// </summary>
        public NewsItemStatusServiceTests()
        {
            NewsItemStatusService = new NewsItemStatusService();
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
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(default, NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.FAULTY_ID } };
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public async Task Add_EmptyClass_ReturnsDEFAULT()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(null);

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.DEFAULT_OBJECT } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_OutOfRangeEnum_ReturnsINCORRECT_STATUS_VALUE()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, (NewsItemStatus)100));

            var expectedResult = new Dictionary<bool, string>() { { false, NewsItemStatusValues.INCORRECT_STATUS_VALUE } };
            Assert.Equal(expectedResult, result);
        }


        // Tests for the different enum values status can have
        [Fact]
        public async Task Add_StatusDone_ReturnsStatusChangeToDone()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Done } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusDispose_ReturnsStatusChangeToDispose()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Dispose));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Dispose } };
            Assert.Equal(expectedResult, result);
        }

        //TODO Fix this test once the roles have been implemented
        /*
        [Fact]
        public async Task Add_StatusArchived_ReturnsStatusChangeToArchived()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Archived));
            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Archived } };
            Assert.Equal(expectedResult, result);
        }
        */

        [Fact]
        public async Task Add_StatusProduction_ReturnsStatusChangeToProduction()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Production));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Production } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusPublication_ReturnsStatusChangeToPublication()
        {
            // FIXME: Use NieuwsItemHelper
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, NewsItemStatus.Publication));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + NewsItemStatus.Publication } };
            Assert.Equal(expectedResult, result);
        }


    }
}