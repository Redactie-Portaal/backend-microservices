using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using NewsItemService.Types;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsItemService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly PublicationService _publicationService;

        public PublicationController(PublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicationById(int id)
        {
            var publication = await _publicationService.GetById(id);
            if (!publication.SingleOrDefault().Key)
            {
                return NotFound(new { message = " A publication with this id cannot be found." });
            }
            return Ok(publication.SingleOrDefault().Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublication(CreatePublicationDTO createPublicationDTO)
        {
            var publicationDTO = await _publicationService.Create(createPublicationDTO);
            if (publicationDTO == null) return BadRequest(new { message = "Publication could not be created." });

            return Ok(publicationDTO);
        }

        [HttpPost("publish")]
        public async Task<IActionResult> Publicize(PublicizeNewsItemDTO dto)
        {
            var newsItem = await _publicationService.PublishNewsItem(dto.NewsItemID, dto.PublicationID);
            if (!newsItem.SingleOrDefault().Key && newsItem.SingleOrDefault().Value == ErrorType.NEWS_ITEM_NOT_FOUND)
            {
                return NotFound(new { message = "NewsItem cannot be found." });
            }
            else
            {
                return Ok(new { message = "Newsitem publishes to specified publication." });
            }
        }
    }
}
