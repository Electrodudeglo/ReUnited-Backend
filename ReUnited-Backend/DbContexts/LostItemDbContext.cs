using ReUnited_Backend.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ReUnited_Backend.DbContexts
{
    public class LostItemDbContext : DbContext
    {
        public DbSet<LostItem> LostItems { get; set; }

        public LostItemDbContext(DbContextOptions<LostItemDbContext> options) 
            : base(options) 
        {

        }
    }
}
