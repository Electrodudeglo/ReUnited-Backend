namespace ReUnited_Backend.Repositories
{
    using DbContexts;
    using DataModels;
    using ReUnited_Backend.DTOs;

    public interface ILostItemRepository
    {
        public IEnumerable<LostItem> GetLostItems();
        public LostItem? GetLostItemById(int id);
        public LostItem? UpdateLostItemById(UpdateLostItemDTO lostItem, int id);
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


        public LostItem? GetLostItemById(int id)
        {
            return _dbContext.LostItems.FirstOrDefault(l => l.Id == id);
        }

        public LostItem? UpdateLostItemById(UpdateLostItemDTO dto, int id)
        {
            var currentLostItem = _dbContext.LostItems.FirstOrDefault(x => x.Id == id);
            if (currentLostItem != null)
            {
                _dbContext.Entry(currentLostItem).CurrentValues.SetValues(dto);
                _dbContext.SaveChanges();
                return currentLostItem;
            }

            return null;
        }
    }
}
