using Microsoft.AspNetCore.Mvc;
using NewsFeedService.DTOs;
using NewsFeedService.Interfaces;
using NewsFeedService.Services;

namespace NewsFeedService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsFeedController : ControllerBase
    {
        private readonly NewsFeedsService _newsFeedsService;
        private readonly INewsFeedRepository _newsFeedRepository;
        public NewsFeedController(INewsFeedRepository newsFeedRepository)
        {
            _newsFeedRepository = newsFeedRepository;
            _newsFeedsService = new NewsFeedsService();
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedsOverview([FromQuery]FeedsParameters feedsParameters)
        {
            var result = await _newsFeedRepository.GetFeeds(feedsParameters);

            return Ok();
        }
    }
}
