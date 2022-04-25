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
        }

        [HttpGet("/author/{id}")]
        public IEnumerable<GetNewsItemDTO> Get(int id)
        {
            this._newsItemOverviewService = new NewsItemOverviewService(this._newsItemRepository);
            return this._newsItemOverviewService.GetNewsItems(id);
        }
    }
}
