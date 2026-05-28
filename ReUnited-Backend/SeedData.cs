using Microsoft.EntityFrameworkCore;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DbContexts;
using System.Text.Json;

namespace ReUnited_Backend
{
    public class SeedData
    {
        private static readonly string _filePath = (".\\Resources\\LostItems.json");
        public static void Initialize(LostItemDbContext lostItemDbContext)
        {
            if (lostItemDbContext.LostItems.Any()) return;

            var lostItems = Utils.GetFileContent<LostItem>(_filePath);

            lostItemDbContext.LostItems.AddRange(lostItems);
            lostItemDbContext.SaveChanges();
        }
    }
}
