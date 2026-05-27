namespace ReUnited_Backend.Repository
{

    public interface ILostItemRepository
    {
    

      
    }

    public class LostItemRepository : ILostItemRepository
    {

        private readonly LostItemDbContext _dbContext;

        public LostItemRepository(LostItemDbContext context)
        {
            _dbContext = context;
        }
    }
}
