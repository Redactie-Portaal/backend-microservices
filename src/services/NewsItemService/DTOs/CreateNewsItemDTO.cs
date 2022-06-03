using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class CreateNewsItemDTO
    {
        [Required]
        [MinLength(1)]
        public List<int> AuthorIds { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public List<int>? CategoryIds { get; set; }
        public List<int>? TagIds { get; set; }
        public List<int> PublicationIds { get; set; }

        public List<AddMediaDTO> MediaDTOs { get; set; }
        public List<AddSourceLocationDTO> SourceLocationDTOs { get; set; }
        public List<AddSourcePersonDTO> SourcePersonDTOs { get; set; }

        public AddNoteDTO? NoteDTO { get; set; }
    }
}
