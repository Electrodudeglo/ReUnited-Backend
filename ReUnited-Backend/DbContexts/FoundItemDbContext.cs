using ReUnited_Backend.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ReUnited_Backend.DbContexts
{
    public class FoundItemDbContext : DbContext
    {
        public DbSet<FoundItem> FoundItems { get; set; }

        public FoundItemDbContext(DbContextOptions<FoundItemDbContext> options) 
            : base(options) 
        {

        }
    }
}
