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
        private readonly IMediaRepository _mediaRepostiory;
        private readonly IPublicationRepository _publicationRepository;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IMessageProducer _producer;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly PublicationService _publicationService;

        public PublicationController(IPublicationRepository publicationRepository, INewsItemRepository newsItemRepository, IMessageProducer producer, IMediaNewsItemRepository mediaNewsItemRepository)
        {
            _publicationRepository = publicationRepository;
            _newsItemRepository = newsItemRepository;
            _mediaNewsItemRepository = mediaNewsItemRepository;
            _producer = producer;
            _publicationService = new PublicationService(_newsItemRepository, _publicationRepository, _mediaNewsItemRepository, _producer);
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
        public async Task<IActionResult> Publicize(PublicizeNewsItemDTO dto)
        {
            var newsItem = await _publicationService.PublishNewsItem(dto.NewsItemID, dto.PublicationID);
            if (!newsItem.SingleOrDefault().Key && newsItem.SingleOrDefault().Value == "NEWSITEMNOTFOUND")
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
