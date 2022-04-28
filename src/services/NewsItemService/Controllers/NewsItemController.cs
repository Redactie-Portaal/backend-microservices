
﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly NewsItemsService service;
        private readonly INewsItemRepository _newsItemRepository;

        public NewsItemController(INewsItemRepository newsItemRepository, NewsItemsService service)
        {
            _newsItemRepository = newsItemRepository;
            _newsItemStatusService = new NewsItemStatusService();
            this.service = service;
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

        [HttpPost]
        public IActionResult Create([FromBody] CreateNewsItemDTO dto)
        {
            bool createSuccess = service.CreateNewsItem(dto);
            if (createSuccess)
                return StatusCode(201);
            else
            {
                return BadRequest();
            }
        }
    }
}
