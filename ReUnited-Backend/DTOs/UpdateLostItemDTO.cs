using System.ComponentModel.DataAnnotations;

namespace ReUnited_Backend.DTOs
{
    public class UpdateLostItemDTO : LostItemBaseDTO
    {
        [Required]
        public string Picture { get; set; } = string.Empty;

        public UpdateLostItemDTO(
            string city,
            string postcode,
            string email,
            string phoneNumber,
            string category,
            string itemDescription,
            string additionalInformation,
            string picture)
        {
            City = city;
            Postcode = postcode;
            Email = email;
            PhoneNumber = phoneNumber;
            Category = category;
            ItemDescription = itemDescription;
            AdditionalInformation = additionalInformation;
            Picture = picture;
        }

    }
}
