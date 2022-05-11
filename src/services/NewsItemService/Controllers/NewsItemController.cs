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
        public async Task<IActionResult> GetByAuthorId(int id)
        {
            if (id < 1) return BadRequest(new { message = "Given author id is not valid, id cannot be smaller than 1." });
            var newsItems = await this._newsItemOverviewService.GetNewsItemsByAuthor(id);
            if (newsItems == null) return NotFound(new { message = "Given author id does not exist." });
            return Ok(newsItems);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] string beforeDate, [FromQuery] string afterDate, [FromQuery] string duringData)
        {
            if (beforeDate != string.Empty)
            {
                DateTime date;
                if (DateTime.TryParse(beforeDate, out date))
                {
                    var newsItems = await this._newsItemOverviewService.GetNewsItemsBeforeDate(date);
                    if (newsItems == null) return NotFound(new { message = "No news items found from before given date." });
                    return Ok(newsItems);
                }
                else
                {
                    return BadRequest(new { message = "No valid date given." });
                }
            }
            else if (afterDate != string.Empty)
            {
                throw new NotImplementedException();
            }
            else if (duringData != string.Empty)
            {
                throw new NotImplementedException();
            }
            else
            {
                return BadRequest(new { message = "No parameters given in the url." });
            }
        }
    }
}
