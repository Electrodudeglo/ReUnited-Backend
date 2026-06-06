using Microsoft.EntityFrameworkCore;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DbContexts;
using System.Text.Json;

namespace ReUnited_Backend
{
    public class SeedData
    {
        private static readonly string _filePath = (".\\Resources\\FoundItems.json");
        public static void Initialize(FoundItemDbContext foundItemDbContext)
        {
            if (foundItemDbContext.FoundItems.Any()) return;

            var foundItems = Utils.GetFileContent<FoundItem>(_filePath);

            foundItemDbContext.FoundItems.AddRange(foundItems);
            foundItemDbContext.SaveChanges();
        }
    }
}
