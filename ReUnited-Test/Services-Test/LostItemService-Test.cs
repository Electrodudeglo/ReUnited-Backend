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

        _lostItemRepoMoq.Setup(r => r.GetLostItems()).Returns(lostItems);
        IEnumerable<LostItem> actual = _lostItemService.GetLostItems();

        Assert.That(actual, Is.EqualTo(lostItems));
    }

    [Test]
    public void GetOneItem_return_One_Item()
    {
        LostItem lostItem = new LostItem();
        _lostItemRepoMoq.Setup(r => r.GetLostItemById(1)).Returns(lostItem);
        LostItem actual = _lostItemService.GetLostItemsById(1);
        Assert.That(actual, Is.EqualTo(lostItem));
    }

    [Test]
    public void AddOneLostItem_Returns_Ok_With_Added_Item()
    {
        LostItem addLostItem = new LostItem();
        _lostItemRepoMoq.Setup(a => a.AddOneLostItem(addLostItem)).Returns(addLostItem);
        LostItem actual = _lostItemService.AddOneLostItem(addLostItem);
        Assert.That(actual, Is.EqualTo(addLostItem));
    }
}
