
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
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly ISourceLocationRepository _sourceLocationRepository;
        private readonly ISourcePersonRepository _sourcePersonRepository;
        private readonly INoteRepository _noteRepository;

        public NewsItemController(INewsItemRepository newsItemRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, IPublicationRepository publicationRepository, ITagRepository tagRepository, IMediaRepository mediaRepository, IMediaNewsItemRepository mediaNewsItemRepository, ISourceLocationRepository sourceLocationRepository, ISourcePersonRepository sourcePersonRepository, INoteRepository noteRepository)
        {
            _newsItemRepository = newsItemRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publicationRepository = publicationRepository;
            _tagRepository = tagRepository;
            _mediaRepository = mediaRepository;
            _mediaNewsItemRepository = mediaNewsItemRepository;
            _sourceLocationRepository = sourceLocationRepository;
            _sourcePersonRepository = sourcePersonRepository;
            _noteRepository = noteRepository;

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
    }
}
