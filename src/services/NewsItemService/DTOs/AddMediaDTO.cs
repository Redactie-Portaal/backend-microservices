using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class AddMediaDTO
    {
        [Required]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public string FileContent { get; set; } = string.Empty;
        [Required]
        public bool isSource { get; set; }
    }
}
