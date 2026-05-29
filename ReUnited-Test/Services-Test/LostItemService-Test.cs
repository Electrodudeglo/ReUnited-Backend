using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
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
    public void UpdateLostItemById_CallsRepositoryOnce()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet",
            "Lost near station",
            "wallet.jpg"
        );

        var lostItem = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet",
            "Lost near station",
            "wallet.jpg"
        );

        _lostItemRepoMoq
            .Setup(r => r.UpdateLostItemById(dto, 1))
            .Returns(lostItem);

        // Act
        _lostItemService.UpdateLostItemById(dto, 1);

        // Assert
        _lostItemRepoMoq.Verify(
            r => r.UpdateLostItemById(dto, 1),
            Times.Once);
    }

    [Test]
    public void UpdateLostItemById_ReturnsUpdatedLostItem()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet",
            "Lost near station",
            "wallet.jpg"
        );

        var expected = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet",
            "Lost near station",
            "wallet.jpg"
        );

        _lostItemRepoMoq
            .Setup(r => r.UpdateLostItemById(dto, 1))
            .Returns(expected);

        // Act
        var result = _lostItemService.UpdateLostItemById(dto, 1);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void UpdateLostItemById_ReturnsNull_WhenRepositoryReturnsNull()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet",
            "Lost near station",
            "wallet.jpg"
        );

        _lostItemRepoMoq
            .Setup(r => r.UpdateLostItemById(dto, 1))
            .Returns((LostItem)null);

        // Act
        var result = _lostItemService.UpdateLostItemById(dto, 1);

        // Assert
        Assert.That(result, Is.Null);

    }

    [Test]
    public void UpdateLostItemById_PassesCorrectArgumentsToRepository()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "Manchester",
            "M11AA",
            "test@test.com",
            "07123456789",
            "Phone",
            "iPhone 15",
            "Blue case",
            "phone.jpg"
        );

        var lostItem = new LostItem(
            "Manchester",
            "M11AA",
            "test@test.com",
            "07123456789",
            "Phone",
            "iPhone 15",
            "Blue case",
            "phone.jpg"
        );

        _lostItemRepoMoq
            .Setup(r => r.UpdateLostItemById(dto, 5))
            .Returns(lostItem);

        // Act
        _lostItemService.UpdateLostItemById(dto, 5);

        // Assert
        _lostItemRepoMoq.Verify(r =>
            r.UpdateLostItemById(
                It.Is<UpdateLostItemDTO>(x =>
                    x.City == "Manchester" &&
                    x.Category == "Phone"),
                5),
            Times.Once);
    }

}
