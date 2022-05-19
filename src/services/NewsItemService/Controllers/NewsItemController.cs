
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Services;

namespace NewsArticleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsItemController : ControllerBase
    {
        //TODO: remove this line that belongs to another branch
        private readonly NewsItemsService newsItemService;
        private readonly INewsItemRepository _newsItemRepository;

        public NewsItemController(INewsItemRepository newsItemRepository)
        {
            _newsItemRepository = newsItemRepository;
            newsItemService = new NewsItemsService(newsItemRepository);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewsItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await newsItemService.CreateNewsItem(dto);
            if (!result.FirstOrDefault().Key)
            return BadRequest(new { message = result.SingleOrDefault().Value });
            else
            {
                return Created("", new { message = result.SingleOrDefault().Value });
            }
        }
    }
}
