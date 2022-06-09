using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsItemService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IMessageProducer _producer;
        private readonly PublicationService _publicationService;

        public PublicationController(IPublicationRepository publicationRepository, INewsItemRepository newsItemRepository, IMessageProducer producer)
        {
            _publicationRepository = publicationRepository;
             _newsItemRepository = newsItemRepository;
            _producer = producer;
            _publicationService = new PublicationService(_publicationRepository, _newsItemRepository);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicationById(int id)
        {
            if (id > 0)
            {
                try
                {
                    var publication = await _publicationRepository.GetPublicationById(id);
                    return Ok(publication.SingleOrDefault().Value);
                }
                catch (Exception e)
                {
                    return Problem("Error:" + e.Message);
                }
            };
            return BadRequest(new { message = "ID cannot be smaller than one." });
        }

        [HttpPost]
        public async Task<IActionResult> Publicize(PublicizeNewsItemDTO dto)
        {
            // TODO: retrieve a news item with the NewsItemService
           var newsItem = await _publicationService.Publicize(dto.NewsItemID, dto.PublicationID);
            if (newsItem == null)
            {

                return BadRequest(); // TODO: Better response
            }
            else
            {
                _producer.PublishMessageAsync(RoutingKeyType.NewsItemPublishTwitter, newsItem);
                return Ok();
            }
        }
    }
}
