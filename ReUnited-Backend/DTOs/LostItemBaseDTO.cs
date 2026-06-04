using System.ComponentModel.DataAnnotations;

namespace ReUnited_Backend.DTOs
{
    public class LostItemBaseDTO
    {
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Postcode { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(11, MinimumLength = 11)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ItemDescription { get; set; } = string.Empty;

        [Required]
        public string AdditionalInformation { get; set; } = string.Empty;
    }
}
