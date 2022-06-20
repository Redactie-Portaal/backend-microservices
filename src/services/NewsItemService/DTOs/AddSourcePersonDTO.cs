using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class AddSourcePersonDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
