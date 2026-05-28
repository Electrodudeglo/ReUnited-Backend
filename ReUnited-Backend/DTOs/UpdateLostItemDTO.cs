using System.ComponentModel.DataAnnotations;

namespace ReUnited_Backend.DTOs
{
    public class UpdateLostItemDTO
    {
        public string City { get; set; } 

        public string Postcode { get; set; } 

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Category { get; set; } 

        public string ItemDescription { get; set; } 

        public string AdditionalInformation { get; set; } 

        public string Picture { get; set; } 

    }
}
