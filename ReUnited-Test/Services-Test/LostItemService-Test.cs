using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.Repositories;
using ReUnited_Backend.Services;

namespace ReUnited_Test;

public class LostItemService_Test
{

    private LostItemService _lostItemService;
    private Mock<ILostItemRepository> _lostItemRepoMoq;

    [SetUp]
    public void Setup()
    {
        _lostItemRepoMoq = new Mock<ILostItemRepository>();
        _lostItemService = new LostItemService(_lostItemRepoMoq.Object);  
    }

    [Test]
    public void GetAllItems_Returns_List_Of_Items()
    {
        List<LostItem> lostItems = new List<LostItem>
        {
            new LostItem(),
            new LostItem(),
        };

        _lostItemRepoMoq.Setup(r => r.GetAllItems()).Returns(lostItems);
        IEnumerable<LostItem> actual = _lostItemService.GetAllItems();

        Assert.That(actual, Is.EqualTo(lostItems));
    }

    [Test]
    public void GetOneItem_return_One_Item()
    {
        LostItem lostItem = new LostItem();
        _lostItemRepoMoq.Setup(r => r.GetOneItem(1)).Returns(lostItem);
        LostItem actual = _lostItemService.GetOneItem(1);
        Assert.That(actual, Is.EqualTo(lostItem));
    }

}
