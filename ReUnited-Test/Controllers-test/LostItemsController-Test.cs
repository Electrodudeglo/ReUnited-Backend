using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.Controllers;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.Services;

namespace ReUnited_Test;

public class LostItemsController_Test
{

    private LostItemsController _lostItemController;
    private Mock<ILostItemService> _lostItemsServiceMoq;
   
    [SetUp]
    public void Setup()
    {
        _lostItemsServiceMoq = new Mock<ILostItemService>();
        _lostItemController = new LostItemsController(_lostItemsServiceMoq.Object);
     
    }

    [Test]
    public void GetAllItems_Returns_Ok_With_List_Of_Items()
    {
        List<LostItem> lostItems = new List<LostItem>
        {
            new LostItem(),
            new LostItem()
        };
        
        _lostItemsServiceMoq.Setup(s => s.GetLostItems()).Returns(lostItems);
        
        OkObjectResult? result = _lostItemController.GetLostItems() as OkObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(result.Value, Is.EqualTo(lostItems));
    }

    [Test]
    public void GetOneItem_Returns_Ok_With_One_Item()
    {
        LostItem oneLostItem = new LostItem();

        _lostItemsServiceMoq.Setup(s => s.GetLostItemsById(1)).Returns(oneLostItem);

        OkObjectResult? result = _lostItemController.GetLostItemById(1) as OkObjectResult;

        Assert.That(result.Value, Is.EqualTo(oneLostItem));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}
