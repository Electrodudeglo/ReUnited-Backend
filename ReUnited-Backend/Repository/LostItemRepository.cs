namespace ReUnited_Backend.Repository
{
    using DbContexts;
    using DataModels;

    public interface ILostItemRepository
    {

        public IEnumerable<LostItem> GetAllItems();
            

      
    }

    public class LostItemRepository : ILostItemRepository
    {

        private readonly LostItemDbContext _dbContext;

        public LostItemRepository(LostItemDbContext context)
        {
            _dbContext = context;
        }

       public IEnumerable<LostItem> GetAllItems()
        {
            return _dbContext.LostItems.ToList();
        }
    }
}
