using Microsoft.AspNetCore.Mvc;
using NewsItemService.Data;
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
    public class NewsItemController : ControllerBase
    {
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly IMessageProducer _producer;
        private INewsItemRepository _newsItemRepository;

        public NewsItemController(IMessageProducer producer ,INewsItemRepository newsItemRepository)
        {
            _producer = producer;
            _newsItemRepository = newsItemRepository;
            _newsItemStatusService = new NewsItemStatusService();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewsItemStatus(AddNewsItemStatusDTO status)
        {
            // Call to service to check if field is empty
            var check = _newsItemStatusService.CheckNewsItemValue(status);
            if (!check.FirstOrDefault().Key)
            {
                return BadRequest(new { message = check.FirstOrDefault().Value });
            }

            // Do database actions
            var result = await _newsItemRepository.ChangeNewsItemStatus(status);
            if (!result.FirstOrDefault().Key)
            {
                return BadRequest(new { message = result.FirstOrDefault().Value });
            }

            //RABBITMQ CALLS IF STATUS MESSAGE IS DISPOSE OR ARCHIVED
            if (status.status == NewsItemStatus.Dispose)
            {
                var newsItem = await _newsItemRepository.GetNewsItemAsync(status.NewsItemId);
                if (newsItem == default)
                {
                    return BadRequest(new { message = result.FirstOrDefault().Value });
                }

                _producer.PublishMessageAsync(RoutingKeyType.NewsItemDispose, _newsItemStatusService.NewsItemToDisposedDTO(newsItem));
            }
            

            // Return Ok message that status has been changed
            return Ok(new { message = result.FirstOrDefault().Value });
        }
    }
}
