using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private NewsItemOverviewService _newsItemOverviewService;
        private readonly INewsItemRepository _newsItemRepository;

        public NewsItemController(INewsItemRepository newsItemRepository)
        {
            this._newsItemRepository = newsItemRepository;
            this._newsItemOverviewService = new NewsItemOverviewService(this._newsItemRepository);
        }

        [HttpGet("author/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1) return BadRequest(new { message = "Given author id is not valid, id cannot be smaller than 1." });
            var newsItems = await this._newsItemOverviewService.GetNewsItems(id);
            if (newsItems == null) return NotFound(new { message = "Given author id does not exist." });
            return Ok(newsItems);
        }
    }
}
