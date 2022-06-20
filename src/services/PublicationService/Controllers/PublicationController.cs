using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicationService.DTOs;
using PublicationService.Interfaces;
using PublicationService.Services;

namespace PublicationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private TwitterService _twitterService;

        public PublicationController(TwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        [HttpPost]
        public async Task<IActionResult> PublishToTwitter(PublishNewsItemDTO publishNewsItemDTO)
        {
            await _twitterService.PublishNewsItem(publishNewsItemDTO);

            return Ok("Item published to Twitter");
        }
    }
}
