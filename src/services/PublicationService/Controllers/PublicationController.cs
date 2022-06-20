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
        private readonly IMediaProvider _mediaProvider;
        private readonly ILogger<TwitterService> _twitterLogger;

        public PublicationController(IMediaProvider mediaProvider, ILogger<TwitterService> twitterLogger)
        {
            this._mediaProvider = mediaProvider;
            _twitterLogger = twitterLogger;
        }

        [HttpPost]
        public async Task<IActionResult> PublishToTwitter(PublishNewsItemDTO publishNewsItemDTO)
        {
            _twitterService = new TwitterService(_mediaProvider, _twitterLogger);
            await this._twitterService.PublishNewsItem(publishNewsItemDTO);
            return this.Ok();
        }
    }
}
