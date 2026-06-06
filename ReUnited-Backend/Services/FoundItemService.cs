using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
using ReUnited_Backend.Repositories;

namespace ReUnited_Backend.Services
{
    public interface IFoundItemService
    {
        public IEnumerable<FoundItem> GetFoundItems();
        public FoundItem GetFoundItemsById(int id);
        public FoundItem UpdateFoundItemById(UpdateFoundItemDTO foundItem, int id);
        public bool DeleteFoundItemById(int id);
        public FoundItem AddOneFoundItem(FoundItem foundItem);
    }

    public class FoundItemService : IFoundItemService
    {
        private readonly IFoundItemRepository _foundItemRepository;

        public FoundItemService(IFoundItemRepository foundItemRepository)
        {
            _foundItemRepository = foundItemRepository;
        }


        public IEnumerable<FoundItem> GetFoundItems()
        {
            return _foundItemRepository.GetFoundItems();
        }

        public FoundItem? GetFoundItemsById(int id)
        {
            return _foundItemRepository.GetFoundItemById(id);
        }

        public FoundItem AddOneFoundItem(FoundItem foundItem)
        {
            return _foundItemRepository.AddOneFoundItem(foundItem);         
        }

        public FoundItem? UpdateFoundItemById(UpdateFoundItemDTO foundItem, int id)
        {
            return _foundItemRepository.UpdateFoundItemById(foundItem, id);
        }
        public bool DeleteFoundItemById(int id)
        {
            return _foundItemRepository.DeleteFoundItemById(id);
        }
    }
}
