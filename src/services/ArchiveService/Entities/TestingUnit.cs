using System.ComponentModel.DataAnnotations;

namespace ArchiveService.Entities
{
    public class TestingUnit
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
