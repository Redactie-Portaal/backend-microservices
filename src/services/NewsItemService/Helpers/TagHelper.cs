using NewsItemService.DTOs;
using NewsItemService.Entities;

namespace NewsItemService.Helpers
{
    public class TagHelper
    {
        public static TagDTO ToDTO(Tag tag)
        {
            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public static Tag ToEntity(TagDTO tagDTO)
        {
            return new Tag
            {
                Id = tagDTO.Id,
                Name = tagDTO.Name
            };
        }

        public static Tag ToEntity(CreateTagDTO tagDTO)
        {
            return new Tag
            {
                Name = tagDTO.Name
            };
        }
    }
}