using Microsoft.AspNetCore.Mvc;
using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsItemController : ControllerBase
    {
        private readonly NewsItemStatusService _newsItemStatusService;
        private INewsItemRepository _newsItemRepository;

        public NewsItemController(INewsItemRepository newsItemRepository)
        {
            _newsItemRepository = newsItemRepository;
            _newsItemStatusService = new NewsItemStatusService();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeNewsItemStatus(int newsItemID)
        {
            // Call to service to check if field is empty
            var check = _newsItemStatusService.CheckNewsItemValue(newsItemID);

            // Check if they passed the test
            if (check.FirstOrDefault().Key)
            {
                return BadRequest(check.FirstOrDefault().Value);
            }

            // Do database actions
            var result = await _newsItemRepository.ChangeNewsItemStatus(newsItemID);

            // Return Ok message that status has been changed
            return Ok(result.FirstOrDefault().Value);
        }
    }
}
