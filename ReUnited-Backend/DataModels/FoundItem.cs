namespace ReUnited_Backend.DataModels
{
    using System.ComponentModel.DataAnnotations;
    public class FoundItem
    {
        public FoundItem()
        {
        }
        public FoundItem(
            string city,
            string postcode,
            string email,
            string phoneNumber,
            string category,
            string itemDescription,
            string additionalInformation,
            string picture,
            string userId)
        {
            City = city;
            Postcode = postcode;
            Email = email;
            PhoneNumber = phoneNumber;
            Category = category;
            ItemDescription = itemDescription;
            AdditionalInformation = additionalInformation;
            Picture = picture;
            UserId = userId;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateOnly DateFound { get; set; }

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

        [Required]
        public string Picture { get; set; } = string.Empty;


    }
}
