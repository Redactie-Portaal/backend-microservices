using Microsoft.AspNetCore.Mvc;
using NewsItemService.DTOs;
using NewsItemService.Helpers;
using NewsItemService.Services;

namespace NewsItemService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTagDTO createTagDTO)
        {
            var tag = _tagService.Post(createTagDTO);
            if (tag == null) return BadRequest("Could not create tag.");

            return Ok(tag);
        }
    }
}