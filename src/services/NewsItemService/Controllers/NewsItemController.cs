using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Services;
using NewsItemService.Interfaces;
using RabbitMQLibrary.Producer;
using NewsItemService.Types;
using RabbitMQLibrary;

namespace NewsArticleService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        //TODO: remove this line that belongs to another branch
        private readonly NewsItemsService newsItemService;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly NewsItemOverviewService _newsItemOverviewService;
        private readonly AuthorService _authorService;
        private readonly NewsItemStatusService _newsItemStatusService;
        private readonly IMessageProducer _producer;
        private readonly IPublicationRepository _publicationRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly ISourceLocationRepository _sourceLocationRepository;
        private readonly ISourcePersonRepository _sourcePersonRepository;
        private readonly INoteRepository _noteRepository;
        
        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public NewsItemController(IMessageProducer producer,
                                  INewsItemRepository newsItemRepository,
                                  IAuthorRepository authorRepository,
                                  ICategoryRepository categoryRepository,
                                  IPublicationRepository publicationRepository,
                                  ITagRepository tagRepository,
                                  IMediaRepository mediaRepository,
                                  IMediaNewsItemRepository mediaNewsItemRepository,
                                  ISourceLocationRepository sourceLocationRepository,
                                  ISourcePersonRepository sourcePersonRepository,
                                  INoteRepository noteRepository,
                                  NewsItemOverviewService newsItemOverviewService, 
                                  AuthorService authorService)
        {
            _newsItemOverviewService = newsItemOverviewService;
            _authorService = authorService;
            _producer = producer;
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

            _newsItemStatusService = new NewsItemStatusService();
            newsItemService = new NewsItemsService(_newsItemRepository, _authorRepository, _categoryRepository, _publicationRepository, _tagRepository, _mediaRepository, _mediaNewsItemRepository, _sourceLocationRepository, _sourcePersonRepository, _noteRepository);
        }

        [HttpGet]
        public IActionResult Get(int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            var newsItems = _newsItemOverviewService.Get(page, pageSize);

            return Ok(newsItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id < 1) return BadRequest(new { message = "ID cannot be smaller than one." });

            var newsItemDTO = this._newsItemOverviewService.Get(id);
            if (newsItemDTO == null) return NotFound(new { message = "News item with given id does not exist." });

            return Ok(newsItemDTO);
        }

        [HttpGet("before")]
        public IActionResult GetBefore(DateTime date, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (date == DateTime.MinValue) return BadRequest(new { message = "Given date is not valid" });
            
            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetBefore(date, page, pageSize));
        }

        [HttpGet("after")]
        public IActionResult GetAfter(DateTime date, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (date == DateTime.MinValue) return BadRequest(new { message = "Given date is not valid" });

            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetAfter(date, page, pageSize));
        }

        [HttpGet("between")]
        public IActionResult GetBetween(DateTime startDate, DateTime endDate, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (startDate == DateTime.MinValue) return BadRequest(new { message = "Given startDate is not valid" });
            if (endDate == DateTime.MinValue) return BadRequest(new { message = "Given endDate is not valid" });

            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            return Ok(_newsItemOverviewService.GetBetween(startDate, endDate, page, pageSize));
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

        [HttpPatch]
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
