using System.ComponentModel.DataAnnotations;

namespace NewsItemService.DTOs
{
    public class AddSourceLocationDTO
    {
        [Required]
        public string StreetName { get; set; } = string.Empty;
        [Required]
        public string HouseNumber { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Province { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
