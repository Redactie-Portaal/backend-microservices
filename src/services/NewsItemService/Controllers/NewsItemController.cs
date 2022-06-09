using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private readonly NewsItemOverviewService _newsItemOverviewService;
        private readonly AuthorService _authorService;
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly IMessageProducer _producer;
        private INewsItemRepository _newsItemRepository;

        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public NewsItemController(NewsItemOverviewService newsItemOverviewService, AuthorService authorService, IMessageProducer producer ,INewsItemRepository newsItemRepository)
        {
            _newsItemOverviewService = newsItemOverviewService;
            _authorService = authorService;
            _producer = producer;
            _newsItemRepository = newsItemRepository;
            _newsItemStatusService = new NewsItemStatusService();
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

            var newsItemDTO = this._newsItemOverviewService.Get(id);
            if (newsItemDTO == null) return NotFound(new { message = "News item with given id does not exist." });

            return Ok(newsItemDTO);
        }

        [HttpPost]
        public IActionResult Post([FromBody] createNewsItemDTO newsItemDTO)
        {
            return Ok(_newsItemOverviewService.Post(newsItemDTO));
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
