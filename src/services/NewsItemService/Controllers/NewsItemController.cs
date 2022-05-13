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

        public NewsItemController(NewsItemOverviewService newsItemOverviewService, AuthorService authorService)
        {
            _newsItemOverviewService = newsItemOverviewService;
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var newsItems = _newsItemOverviewService.Get();

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
        public IActionResult GetBefore(DateTime date)
        {
            return Ok(_newsItemOverviewService.GetBefore(date));
        }

        [HttpGet("after")]
        public IActionResult GetAfter(DateTime date)
        {
            return Ok(_newsItemOverviewService.GetAfter(date));
        }

        [HttpGet("between")]
        public IActionResult GetBetween(DateTime startDate, DateTime endDate)
        {
            return Ok(_newsItemOverviewService.GetBetween(startDate, endDate));
        }
    }
}
