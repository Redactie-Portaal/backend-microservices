using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Services;
using NewsItemService.Interfaces;
using RabbitMQLibrary.Producer;
using NewsItemService.Types;
using RabbitMQLibrary;
using NewsItemService.Helpers;

namespace NewsArticleService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private readonly IMessageProducer _producer;
        private readonly NewsItemsService _newsItemsService;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly NewsItemOverviewService _newsItemOverviewService;
        
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public NewsItemController(IMessageProducer producer,
                                  NewsItemsService newsItemsService,
                                  INewsItemRepository newsItemRepository,
                                  NewsItemStatusService newsItemStatusService,
                                  NewsItemOverviewService newsItemOverviewService)
        {
            _producer = producer;
            _newsItemsService = newsItemsService;
            _newsItemRepository = newsItemRepository;
            _newsItemStatusService = newsItemStatusService;
            _newsItemOverviewService = newsItemOverviewService;
        }

        [HttpGet]
        public IActionResult Get(int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            var newsItems = _newsItemOverviewService.Get(page, pageSize);

            return Ok(newsItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id < 1) return BadRequest(new { message = "ID cannot be smaller than one." });

            var newsItemDTO = _newsItemOverviewService.Get(id);
            if (newsItemDTO == null) return NotFound(new { message = "News item with given id does not exist." });

            return Ok(newsItemDTO);
        }

        [HttpGet("before")]
        public IActionResult GetBefore(DateTime date, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (date == DateTime.MinValue) return BadRequest(new { message = "Given date is not valid" });
            
            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetBefore(date, page, pageSize));
        }

        [HttpGet("after")]
        public IActionResult GetAfter(DateTime date, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (date == DateTime.MinValue) return BadRequest(new { message = "Given date is not valid" });

            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetAfter(date, page, pageSize));
        }

        [HttpGet("between")]
        public IActionResult GetBetween(DateTime startDate, DateTime endDate, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (startDate == DateTime.MinValue) return BadRequest(new { message = "Given startDate is not valid" });
            if (endDate == DateTime.MinValue) return BadRequest(new { message = "Given endDate is not valid" });

            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetBetween(startDate, endDate, page, pageSize));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _newsItemsService.CreateNewsItem(dto);
            if (!result.FirstOrDefault().Key)
            {
                return BadRequest(new { message = result.SingleOrDefault().Value });
            }
            else
            {
                return Created("", new { message = result.SingleOrDefault().Value });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> AddNewsItemStatus(AddNewsItemStatusDTO status)
        {
            // Call to service to check if field is empty
            var check = _newsItemOverviewService.CheckNewsItemValue(status);
            if (!check.FirstOrDefault().Key)
            {
                return BadRequest(new { message = check.FirstOrDefault().Value });
            }

            // Do database actions
            // FIXME: Use newsItem service.
            var result = await _newsItemRepository.ChangeNewsItemStatus(status);
            if (!result.FirstOrDefault().Key)
            {
                return BadRequest(new { message = result.FirstOrDefault().Value });
            }

            // RABBITMQ CALLS IF STATUS MESSAGE IS DISPOSE OR ARCHIVED
            if (status.status == NewsItemStatus.Dispose)
            {
                // FIXME: Use newsItem service.
                var newsItem = _newsItemRepository.Get(status.NewsItemId);
                if (newsItem == default)
                {
                    return BadRequest(new { message = result.FirstOrDefault().Value });
                }

                _producer.PublishMessageAsync(RoutingKeyType.NewsItemDispose, NewsItemHelper.NewsItemToDisposedDTO(newsItem));
            }

            // Return Ok message that status has been changed
            return Ok(new { message = result.FirstOrDefault().Value });
        }      
    }
}
