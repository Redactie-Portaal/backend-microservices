
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using NewsItemService.Types;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsArticleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsItemController : ControllerBase
    {
        //TODO: remove this line that belongs to another branch
        private readonly NewsItemsService newsItemService;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly IMessageProducer _producer;
        public NewsItemController(IMessageProducer producer,
                                  INewsItemRepository newsItemRepository,
                                  IAuthorRepository authorRepository,
                                  ICategoryRepository categoryRepository)
        {
            _producer = producer;
            _newsItemRepository = newsItemRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            newsItemService = new NewsItemsService(_newsItemRepository, _authorRepository, _categoryRepository);
            _newsItemStatusService = new NewsItemStatusService();
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


        public async Task<IActionResult> AddNewsItemStatus(AddNewsItemStatusDTO status)
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

            //RABBITMQ CALLS IF STATUS MESSAGE IS DISPOSE OR ARCHIVED
            if (status.status == NewsItemStatus.Dispose)
            {
                var newsItem = await _newsItemRepository.GetNewsItemAsync(status.NewsItemId);
                if (newsItem == default)
                {
                    return BadRequest(new { message = result.FirstOrDefault().Value });
                }

                _producer.PublishMessageAsync(RoutingKeyType.NewsItemDispose, _newsItemStatusService.NewsItemToDisposedDTO(newsItem));
            }


            // Return Ok message that status has been changed
            return Ok(new { message = result.FirstOrDefault().Value });
        }
    }
}
