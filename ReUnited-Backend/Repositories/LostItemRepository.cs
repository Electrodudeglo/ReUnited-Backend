namespace ReUnited_Backend.Repositories
{
    using DbContexts;
    using DataModels;

    public interface ILostItemRepository
    {
        public IEnumerable<LostItem> GetAllItems();
        public LostItem GetOneItem(int id);
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


        public LostItem GetOneItem(int id)
        {
            return _dbContext.LostItems.FirstOrDefault(l => l.Id == id);
        }

    }
}
