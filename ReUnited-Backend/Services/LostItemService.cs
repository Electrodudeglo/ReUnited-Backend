using ReUnited_Backend.DataModels;
using ReUnited_Backend.Repositories;

namespace ReUnited_Backend.Services
{
    public interface ILostItemService
    {
        public IEnumerable<LostItem> GetLostItems();
        public LostItem GetLostItemsById(int id);
    }

    public class LostItemService : ILostItemService
    {
        private readonly ILostItemRepository _lostItemRepository;

        public LostItemService(ILostItemRepository lostItemRepository)
        {
            _lostItemRepository = lostItemRepository;
        }


        public IEnumerable<LostItem> GetLostItems()
        {
            return _lostItemRepository.GetLostItems();
        }

        public LostItem GetLostItemsById(int id)
        {
            return _lostItemRepository.GetLostItemById(id);
        }

    }
}
