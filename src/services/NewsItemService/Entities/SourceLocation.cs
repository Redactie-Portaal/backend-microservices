using System.ComponentModel.DataAnnotations;

namespace NewsItemService.Entities
{
    public class SourceLocation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string StreetName { get; set; } = string.Empty;

        public string HouseNumber { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public ICollection<NewsItem> NewsItems { get; set; }
    }
}
