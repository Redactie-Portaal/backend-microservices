using Microsoft.AspNetCore.Mvc;

namespace NewsFeedService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsFeedController : ControllerBase
    {
        public NewsFeedController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetFeedsOverview()
        {


            return Ok();
        }
    }
}
