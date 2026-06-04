using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ReUnited_Backend.DTOs
{
    public class CreateLostItemDTO : LostItemBaseDTO
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}