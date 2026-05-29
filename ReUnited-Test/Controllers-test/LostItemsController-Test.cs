using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.Controllers;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
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

    [Test]
    public void UpdateLostItemById_ReturnsCreatedResult()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Returns(output);

        // Act
        var result = _lostItemController.UpdateLostItemById(dto, 1);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedResult>());
    }

    [Test]
    public void UpdateLostItemById_CallsServiceOnce()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Returns(output);

        // Act
        _lostItemController.UpdateLostItemById(dto, 1);

        // Assert
        _lostItemsServiceMoq.Verify(
            service => service.UpdateLostItemById(dto, 1),
            Times.Once());
    }

    [Test]
    public void UpdateLostItemById_ReturnsStatusCode201()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Returns(output);

        // Act
        CreatedResult result = (CreatedResult)_lostItemController
            .UpdateLostItemById(dto, 1);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(201));
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
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new LostItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Lost near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Returns(output);

        // Act
        CreatedResult result = (CreatedResult)_lostItemController
            .UpdateLostItemById(dto, 1);

        // Assert
        Assert.That(result.Value, Is.EqualTo(output));
    }

    [Test]
    public void UpdateLostItemById_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john@test.com",
            "07123456789",
            "Wallet",
            "Black wallet",
            "Extra info",
            "wallet.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Returns((LostItem)null);

        // Act
        var result = _lostItemController.UpdateLostItemById(dto, 1);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void UpdateLostItemById_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "",
            "",
            "invalid-email",
            "123",
            "",
            "",
            "",
            ""
        );

        _lostItemController.ModelState.AddModelError(
            "Category",
            "Category is required");

        // Act
        var result = _lostItemController.UpdateLostItemById(dto, 1);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void UpdateLostItemById_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        var dto = new UpdateLostItemDTO(
            "London",
            "SW1A1AA",
            "john@test.com",
            "07123456789",
            "Wallet",
            "Black wallet",
            "Extra info",
            "wallet.jpg"
        );

        _lostItemsServiceMoq
            .Setup(s => s.UpdateLostItemById(dto, 1))
            .Throws(new Exception());

        // Act & Assert
        Assert.Throws<Exception>(() =>
            _lostItemController.UpdateLostItemById(dto, 1));
    public void AddOneLostItem_Returns_Ok_With_Added_Item()
    {
        var newItem = new LostItem();

        _lostItemsServiceMoq.Setup(n => n.AddOneLostItem(newItem)).Returns(newItem);

        CreatedResult? result = _lostItemController.AddOneLostItem(newItem) as CreatedResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(201, result.StatusCode);
        Assert.AreEqual(newItem, result.Value);

    }
}
