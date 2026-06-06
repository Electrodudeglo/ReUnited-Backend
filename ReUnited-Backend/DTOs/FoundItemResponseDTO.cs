namespace ReUnited_Backend.DTOs
{
    public class FoundItemResponseDTO : FoundItemBaseDTO
    {
        public int Id { get; set; }
        public string Picture { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
