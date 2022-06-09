using Microsoft.AspNetCore.Mvc;
using NewsItemService.Entities;
using NewsItemService.DTOs;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        private const int DEFAULT_PAGE = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var authors = _authorService.Get();
            if (authors == null) return NotFound(new { message = "No authors found." });

            return Ok(authors);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var author = _authorService.Get(id);
            if (author == null) return NotFound(new { message = "No author found." });

            return Ok(author);
        }

        [HttpGet("{id}/newsitems")]
        public IActionResult GetNewsItems(int id, int page = DEFAULT_PAGE, int pageSize = DEFAULT_PAGE_SIZE)
        {
            if (page == 0) page = DEFAULT_PAGE;
            if (pageSize == 0 ) pageSize = int.MaxValue;

            var newsItems = _authorService.GetNewsItems(id, page, pageSize);
            if (newsItems == null) return NotFound(new { message = "No news items found." });

            return Ok(newsItems);
        }

        [HttpPost]
        public AuthorDTO Post([FromBody] CreateAuthorDTO author)
        {
            var authorDTO = _authorService.Post(author);
            return authorDTO;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Author author)
        {
            throw new NotImplementedException();
            // Author? authorToUpdate = _context.Authors.Find(id);

            // if (authorToUpdate == null)
            // {
            //     return NotFound();
            // }

            // if (author.Name != null)
            // {
            //     authorToUpdate.Name = author.Name;
            // }

            // _context.SaveChanges();

            // return Ok(authorToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
            // Author? authorToDelete = _context.Authors.Find(id);

            // if (authorToDelete == null)
            // {
            //     return NotFound();
            // }

            // _context.Authors.Remove(authorToDelete);
            // _context.SaveChanges();

            // return Ok(authorToDelete);
        }
    }
}
