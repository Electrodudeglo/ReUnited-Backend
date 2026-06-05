using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.Controllers;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
using ReUnited_Backend.Services;
using Microsoft.Extensions.Options;

namespace ReUnited_Test;

public class FoundItemsController_Test
{

    private FoundItemsController _foundItemController;
    private Mock<IFoundItemService> _foundItemsServiceMock;
    private Mock<IImageStorageService> _imageStorageServiceMock;
    private Mock<ImageUrlService> _imageUrlServiceMock;

    [SetUp]
    public void Setup()
    {
        _foundItemsServiceMock = new Mock<IFoundItemService>();

        _imageStorageServiceMock =
        new Mock<IImageStorageService>();

        var options =
            Options.Create(
                new SupabaseSettings
                {
                    Url = "https://test.com",
                    Bucket = "test-bucket",
                    ApiKey = "test-key"
                });

        var imageUrlService =
            new ImageUrlService(options);

        _foundItemController =
                new FoundItemsController(
                    _foundItemsServiceMock.Object,
                    _imageStorageServiceMock.Object,
                    imageUrlService);
    }

    [Test]
    public void GetAllItems_Returns_Ok_With_List_Of_Items()
    {
        List<FoundItem> foundItems = new List<FoundItem>
        {
            new FoundItem(),
            new FoundItem()
        };

        _foundItemsServiceMock.Setup(s => s.GetFoundItems()).Returns(foundItems);

        OkObjectResult? result = _foundItemController.GetFoundItems() as OkObjectResult;

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(result.Value, Is.EqualTo(foundItems));
    }

    [Test]
    public void GetOneItem_Returns_Ok_With_One_Item()
    {
        FoundItem oneFoundItem = new FoundItem();

        _foundItemsServiceMock.Setup(s => s.GetFoundItemsById(1)).Returns(oneFoundItem);

        OkObjectResult? result = _foundItemController.GetFoundItemById(1) as OkObjectResult;

        Assert.That(result.Value, Is.EqualTo(oneFoundItem));
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
    [Test]
    public void DeleteFoundItem_Returns_NoContent()
    {
        _foundItemsServiceMock.Setup(s => s.DeleteFoundItemById(1)).Returns(true);

        NoContentResult? result = _foundItemController.DeleteFoundItemById(1) as NoContentResult;

        Assert.That(result, Is.TypeOf<NoContentResult>());
        Assert.That(result.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public void DeleteFoundItem_Returns_NotFound()
    {
        _foundItemsServiceMock.Setup(s => s.DeleteFoundItemById(1)).Returns(false);

        NotFoundObjectResult? result = _foundItemController.DeleteFoundItemById(1) as NotFoundObjectResult;

        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        Assert.That(result.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public void UpdateFoundItemById_ReturnsOkResult()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new FoundItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Returns(output);

        // Act
        var result = _foundItemController.UpdateFoundItemById(dto, 1);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void UpdateFoundItemById_CallsServiceOnce()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new FoundItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Returns(output);

        // Act
        _foundItemController.UpdateFoundItemById(dto, 1);

        // Assert
        _foundItemsServiceMock.Verify(
            service => service.UpdateFoundItemById(dto, 1),
            Times.Once());
    }

    [Test]
    public void UpdateFoundItemById_ReturnsStatusCode200()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new FoundItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Returns(output);

        // Act
        OkObjectResult result = (OkObjectResult)_foundItemController
            .UpdateFoundItemById(dto, 1);

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public void UpdateFoundItemById_ReturnsUpdatedFoundItem()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        var output = new FoundItem(
            "London",
            "SW1A1AA",
            "john.doe@example.com",
            "07123456789",
            "Wallet",
            "Black leather wallet with several bank cards inside.",
            "Found near Victoria Station on Tuesday evening.",
            "wallet-image.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Returns(output);

        // Act
        OkObjectResult result = (OkObjectResult)_foundItemController
            .UpdateFoundItemById(dto, 1);

        // Assert
        Assert.That(result.Value, Is.EqualTo(output));
    }

    [Test]
    public void UpdateFoundItemById_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john@test.com",
            "07123456789",
            "Wallet",
            "Black wallet",
            "Extra info",
            "wallet.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Returns((FoundItem)null);

        // Act
        var result = _foundItemController.UpdateFoundItemById(dto, 1);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void UpdateFoundItemById_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "",
            "",
            "invalid-email",
            "123",
            "",
            "",
            "",
            ""
        );

        _foundItemController.ModelState.AddModelError(
            "Category",
            "Category is required");

        // Act
        var result = _foundItemController.UpdateFoundItemById(dto, 1);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void UpdateFoundItemById_Returns500_WhenExceptionOccurs()
    {
        // Arrange
        var dto = new UpdateFoundItemDTO(
            "London",
            "SW1A1AA",
            "john@test.com",
            "07123456789",
            "Wallet",
            "Black wallet",
            "Extra info",
            "wallet.jpg"
        );

        _foundItemsServiceMock
            .Setup(s => s.UpdateFoundItemById(dto, 1))
            .Throws(new Exception());

        // Act
        var result = _foundItemController.UpdateFoundItemById(dto, 1);

        // Assert
        var statusResult = (ObjectResult)result;

        Assert.That(statusResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(statusResult.StatusCode, Is.EqualTo(500));
            Assert.That(
                statusResult.Value,
                Is.EqualTo("An error occurred while updating the found item."));
        });
    }

    //[Test]
    //public void AddOneFoundItem_Returns_Ok_With_Added_Item()
    //{
    //    var newItem = new FoundItem();

    //    _foundItemsServiceMock.Setup(n => n.AddOneFoundItem(newItem)).Returns(newItem);

    //    CreatedResult? result = _foundItemController.AddOneFoundItem(newItem) as CreatedResult;

    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(201, result.StatusCode);
    //    Assert.AreEqual(newItem, result.Value);

    //}
}
