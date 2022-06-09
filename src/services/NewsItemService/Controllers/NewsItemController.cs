
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
        private readonly IPublicationRepository _publicationRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly ISourceLocationRepository _sourceLocationRepository;
        private readonly ISourcePersonRepository _sourcePersonRepository;
        private readonly INoteRepository _noteRepository;

        public NewsItemController(IMessageProducer producer,
                                  INewsItemRepository newsItemRepository,
                                  IAuthorRepository authorRepository,
                                  ICategoryRepository categoryRepository,
                                  ITagRepository tagRepository,
                                  IMediaRepository mediaRepository,
                                  IMediaNewsItemRepository mediaNewsItemRepository,
                                  ISourceLocationRepository sourceLocationRepository,
                                  ISourcePersonRepository sourcePersonRepository,
                                  INoteRepository noteRepository)
        {
            _producer = producer;
            _newsItemRepository = newsItemRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _mediaRepository = mediaRepository;
            _mediaNewsItemRepository = mediaNewsItemRepository;
            _sourceLocationRepository = sourceLocationRepository;
            _sourcePersonRepository = sourcePersonRepository;
            _noteRepository = noteRepository;

            _newsItemStatusService = new NewsItemStatusService();
            newsItemService = new NewsItemsService(_newsItemRepository, _authorRepository, _categoryRepository, _publicationRepository, _tagRepository, _mediaRepository, _mediaNewsItemRepository, _sourceLocationRepository, _sourcePersonRepository, _noteRepository);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsItemDTO dto)
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
