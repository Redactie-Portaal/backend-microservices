using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class TagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public TagDTO? Post(CreateTagDTO createTagDTO)
        {
            var tag = TagHelper.ToEntity(createTagDTO);
            
            tag = _tagRepository.Post(tag);
            if (tag == null) return null;

            return TagHelper.ToDTO(tag);
        }
    }
}