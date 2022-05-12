
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

        [HttpPost]
        public async Task<IActionResult> AddNewsItemStatus(AddNewsItemStatus status)
        {
            // Call to service to check if field is empty
            var check = _newsItemStatusService.CheckNewsItemValue(status);
            if (!check.FirstOrDefault().Key)
            {
                return BadRequest(new { message = check.FirstOrDefault().Value });
            }

            // Do database actions
            var result = await _newsItemRepository.ChangeNewsItemStatus(status);
            if (!result.FirstOrDefault().Key)
            {
                return BadRequest(new { message = result.FirstOrDefault().Value });
            }

            //TODO: INSERT RABBITMQ CALLS IF STATUS MESSAGE IS DISPOSE OR ARCHIVED

            // Return Ok message that status has been changed
            return Ok(new { message = result.FirstOrDefault().Value });
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
