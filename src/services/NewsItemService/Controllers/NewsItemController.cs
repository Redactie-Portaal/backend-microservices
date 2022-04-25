using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private NewsItemOverviewService _newsItemOverviewService = new NewsItemOverviewService();

        [HttpGet("/author/{id}")]
        public IEnumerable<GetNewsItemDTO> Get(string id)
        {
            return _newsItemOverviewService.GetNewsItems(id);
        }
    }
}
