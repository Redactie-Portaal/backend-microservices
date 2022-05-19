using Microsoft.AspNetCore.Mvc;
using NewsItemService.Data;
using NewsItemService.Entities;
using NewsItemService.Helpers;
using NewsItemService.DTOs;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            throw new NotImplementedException();
            // return Ok(AuthorHelper.ToDTO(_context.Authors.ToList()));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
            // var author = _context.Authors.Find(id);
            // if (author == null ) return NotFound();

            // return Ok(AuthorHelper.ToDTO(author));
        }

        [HttpGet("{id}/newsitems")]
        public IActionResult GetNewsItems(int id)
        {
            throw new NotImplementedException();
            // var author = _context.Authors.Find(id);

            // if (author == null) return NotFound($"Author with id {id} not found");
            // if (author.NewsItems == null) return NotFound($"No news items found for author with id {id}");

            // return Ok(NewsItemHelper.ToDTO(author.NewsItems.ToList()));
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
