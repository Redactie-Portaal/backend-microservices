using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicationService.Interfaces;

namespace PublicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService _publicationService;
        private readonly IMediaProvider _mediaProvider;

        public PublicationController(IPublicationService publicationService, IMediaProvider mediaProvider)
        {
            this._publicationService = publicationService;
            this._mediaProvider = mediaProvider;
        }

        [HttpPost]
        public async Task<IActionResult> PublishToTwitter()
        {
            await this._publicationService.PublishStory();
            return this.Ok();
        }
    }
}
