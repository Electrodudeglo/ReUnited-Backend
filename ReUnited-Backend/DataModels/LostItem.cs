namespace ReUnited_Backend.DataModels
{
    using System.ComponentModel.DataAnnotations;
    public class LostItem
    {
        public LostItem()
        {
            City = string.Empty;    
            Postcode = string.Empty;
            Category = string.Empty;    
            ItemDescription = string.Empty;
            Picture = string.Empty;
            
        }
        [Key]
        public int Id { get; set; }

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
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ItemDescription { get; set; } = string.Empty;

        public string AdditionalInformation { get; set; } = string.Empty;

        [Required]
        public string Picture { get; set; }

    }
}
