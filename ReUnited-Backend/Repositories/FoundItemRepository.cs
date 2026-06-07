namespace ReUnited_Backend.Repositories
{
    using DbContexts;
    using DataModels;
    using ReUnited_Backend.DTOs;
    using Microsoft.AspNetCore.Http.HttpResults;

    public interface IFoundItemRepository
    {
        public IEnumerable<FoundItem> GetFoundItems();
        public FoundItem? GetFoundItemById(int id);
        public FoundItem? UpdateFoundItemById(UpdateFoundItemDTO foundItem, int id);
        public bool DeleteFoundItemById(int id);


        public FoundItem AddOneFoundItem(FoundItem foundItem);
    }

    public class FoundItemRepository : IFoundItemRepository
    {
        private readonly FoundItemDbContext _dbContext;
        public FoundItemRepository(FoundItemDbContext context)
        {
            _dbContext = context;
        }

       public IEnumerable<FoundItem> GetFoundItems()
        {
            return _dbContext.FoundItems.ToList();
        }


        public FoundItem? GetFoundItemById(int id)
        {
            return _dbContext.FoundItems.FirstOrDefault(l => l.Id == id);
        }

        public FoundItem AddOneFoundItem(FoundItem foundItem)
        {

            _dbContext.Add(foundItem);
            _dbContext.SaveChanges();
            return foundItem;
   
        }

        public FoundItem? UpdateFoundItemById(UpdateFoundItemDTO dto, int id)
        {
            var currentFoundItem = _dbContext.FoundItems.FirstOrDefault(x => x.Id == id);
            if (currentFoundItem != null)
            {
                currentFoundItem.City = dto.City;
                currentFoundItem.Postcode = dto.Postcode;
                currentFoundItem.Email = dto.Email;
                currentFoundItem.PhoneNumber = dto.PhoneNumber;
                currentFoundItem.Category = dto.Category;
                currentFoundItem.ItemDescription = dto.ItemDescription;
                currentFoundItem.AdditionalInformation = dto.AdditionalInformation;
                
                _dbContext.SaveChanges();
                return currentFoundItem;
            }

            return null;
        }
        public bool DeleteFoundItemById(int id)
        {
            var foundItem = _dbContext.FoundItems.FirstOrDefault(x => x.Id == id);

            if (foundItem == null)
            {
                return false;
            }

            _dbContext.FoundItems.Remove(foundItem);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
