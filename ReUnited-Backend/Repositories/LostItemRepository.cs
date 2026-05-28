namespace ReUnited_Backend.Repositories
{
    using DbContexts;
    using DataModels;

    public interface ILostItemRepository
    {
        public IEnumerable<LostItem> GetLostItems();
        public LostItem GetLostItemById(int id);
    }

    public class LostItemRepository : ILostItemRepository
    {
        private readonly LostItemDbContext _dbContext;
        public LostItemRepository(LostItemDbContext context)
        {
            _dbContext = context;
        }

       public IEnumerable<LostItem> GetLostItems()
        {
            return _dbContext.LostItems.ToList();
        }


        public LostItem GetLostItemById(int id)
        {
            return _dbContext.LostItems.FirstOrDefault(l => l.Id == id);
        }

    }
}
