using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicationService.DTOs;
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
        public async Task<IActionResult> PublishToTwitter(PublishNewsItemDTO publishNewsItemDTO)
        {
            await this._publicationService.PublishNewsItem(publishNewsItemDTO);
            return this.Ok();
        }
    }
}
