using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class AddNoteDTO
    {
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public DateTime Updated { get; set; }
    }
}
