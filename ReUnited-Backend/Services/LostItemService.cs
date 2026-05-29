using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
using ReUnited_Backend.Repositories;

namespace ReUnited_Backend.Services
{
    public interface ILostItemService
    {
        public IEnumerable<LostItem> GetLostItems();
        public LostItem GetLostItemsById(int id);
        public LostItem UpdateLostItemById(UpdateLostItemDTO lostItem, int id);
        public bool DeleteLostItemById(int id);
        public LostItem? GetLostItemsById(int id);
        public LostItem? UpdateLostItemById(UpdateLostItemDTO lostItem, int id);
        public LostItem AddOneLostItem(LostItem lostItem);
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

        public LostItem? GetLostItemsById(int id)
        {
            return _lostItemRepository.GetLostItemById(id);
        }

        public LostItem AddOneLostItem(LostItem lostItem)
        {
            return _lostItemRepository.AddOneLostItem(lostItem);         
        }

        public LostItem? UpdateLostItemById(UpdateLostItemDTO lostItem, int id)
        {
            return _lostItemRepository.UpdateLostItemById(lostItem, id);
        }
        public bool DeleteLostItemById(int id)
        {
            return _lostItemRepository.DeleteLostItemById(id);
        }
    }
}
