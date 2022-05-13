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

        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public NewsItemController(NewsItemOverviewService newsItemOverviewService, AuthorService authorService)
        {
            _newsItemOverviewService = newsItemOverviewService;
            _authorService = authorService;
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
            if (id < 1) return BadRequest(new { message = "Given news item id is not valid, id cannot be smaller than 1." });

            return Ok(this._newsItemOverviewService.Get(id));
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
    }
}
