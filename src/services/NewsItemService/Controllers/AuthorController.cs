using Microsoft.AspNetCore.Mvc;
using NewsItemService.Data;
using NewsItemService.Entities;
using NewsItemService.Helpers;
using NewsItemService.DTOs;

namespace NewsItemService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly NewsItemServiceDatabaseContext _context;

        public AuthorController(NewsItemServiceDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(AuthorHelper.ToDTO(_context.Authors.ToList()));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null ) return NotFound();

            return Ok(AuthorHelper.ToDTO(author));
        }

        [HttpGet("{id}/newsitems")]
        public IActionResult GetNewsItems(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null) return NotFound($"Author with id {id} not found");
            if (author.NewsItems == null) return NotFound($"No news items found for author with id {id}");

            return Ok(NewsItemHelper.ToDTO(author.NewsItems.ToList()));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return Ok(author);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Author author)
        {
            Author? authorToUpdate = _context.Authors.Find(id);

            if (authorToUpdate == null)
            {
                return NotFound();
            }

            if (author.Name != null)
            {
                authorToUpdate.Name = author.Name;
            }

            _context.SaveChanges();

            return Ok(authorToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Author? authorToDelete = _context.Authors.Find(id);

            if (authorToDelete == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(authorToDelete);
            _context.SaveChanges();

            return Ok(authorToDelete);
        }
    }
}
