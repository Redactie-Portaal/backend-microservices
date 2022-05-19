using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class CreateNewsItemDTO
    {
        [Required]
        [MinLength(1)]
        public List<int> AuthorIds { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        public string? LocationInformation { get; set; }
        public string? ContactInformation { get; set; }
        public string? ProductionDate { get; set; }
        public string? Region { get; set; }
        public DateTime CreationDate { get; set; }

        public string? VideoUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public string? ÀudioUrl { get; set; }
    }
}
