namespace ReUnited_Backend.DTOs
{
    public class LostItemResponseDTO : LostItemBaseDTO
    {
        public int Id { get; set; }
        public string Picture { get; set; } = string.Empty;
    }
}
