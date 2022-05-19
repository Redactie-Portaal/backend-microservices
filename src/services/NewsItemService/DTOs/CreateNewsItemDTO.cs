using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class CreateNewsItemDTO
    {
        [Required]
        [MinLength(1)]
        public List<int> AuthorIds { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        public string? LocationInformation { get; set; }
        public string? ContactInformation { get; set; }
        public DateTime ProductionDate { get; set; }
        public string? Region { get; set; }
        public DateTime EndDate { get; set; }

        public List<int> CategoryIds { get; set; }

        public string? VideoUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public string? ÀudioUrl { get; set; }
    }
}
