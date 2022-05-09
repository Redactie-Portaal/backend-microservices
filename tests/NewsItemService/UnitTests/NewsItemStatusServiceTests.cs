using NewsItemService.DTOs;
using NewsItemService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private AddNewsItemStatus CreateAddNewsItemStatus(int ID, Enums.NewsItemStatus status )
        {
            return new AddNewsItemStatus()
            {
                NewsItemId = ID,
                status = status
            };
        }

        [Fact]
        public async Task Add_EmptyInteger_ReturnsFAULTY_ID()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(default, Enums.NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { false, "STATUS.FAULTY_ID" } };
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public async Task Add_EmptyClass_ReturnsDEFAULT()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(null);

            var expectedResult = new Dictionary<bool, string>() { { false, "STATUS.DEFAULT_OBJECT" } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_OutOfRangeEnum_ReturnsINCORRECT_STATUS_VALUE()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, (Enums.NewsItemStatus)100));

            var expectedResult = new Dictionary<bool, string>() { { false, "STATUS.INCORRECT_STATUS_VALUE" } };
            Assert.Equal(expectedResult, result);
        }


        // Tests for the different enum values status can have
        [Fact]
        public async Task Add_StatusDone_ReturnsStatusChangeToDone()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Done));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Done } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusDispose_ReturnsStatusChangeToDispose()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Dispose));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Dispose } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusArchived_ReturnsStatusChangeToArchived()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Archived));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Archived } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusProduction_ReturnsStatusChangeToProduction()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Production));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Production } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Add_StatusPublication_ReturnsStatusChangeToPublication()
        {
            var result = NewsItemStatusService.CheckNewsItemValue(CreateAddNewsItemStatus(1, Enums.NewsItemStatus.Publication));

            var expectedResult = new Dictionary<bool, string>() { { true, "Status able change to " + Enums.NewsItemStatus.Publication } };
            Assert.Equal(expectedResult, result);
        }


    }
}
