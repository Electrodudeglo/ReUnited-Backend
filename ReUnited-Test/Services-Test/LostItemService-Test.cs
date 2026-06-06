using Microsoft.IdentityModel.Tokens;
using Moq;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
using ReUnited_Backend.Repositories;
using ReUnited_Backend.Services;

namespace ReUnited_Test;

public class FoundItemService_Test
{

    private FoundItemService _foundItemService;
    private Mock<IFoundItemRepository> _foundItemRepoMock;

    [SetUp]
    public void Setup()
    {
        _foundItemRepoMock = new Mock<IFoundItemRepository>();
        _foundItemService = new FoundItemService(_foundItemRepoMock.Object);  
    }

    [Test]
    public void GetAllItems_Returns_List_Of_Items()
    {
        List<FoundItem> foundItems = new List<FoundItem>
        {
            new FoundItem(),
            new FoundItem(),
        };

        _foundItemRepoMock.Setup(r => r.GetFoundItems()).Returns(foundItems);
        IEnumerable<FoundItem> actual = _foundItemService.GetFoundItems();

        Assert.That(actual, Is.EqualTo(foundItems));
    }

    [Test]
    public void GetOneItem_return_One_Item()
    {
        FoundItem foundItem = new FoundItem();
        _foundItemRepoMock.Setup(r => r.GetFoundItemById(1)).Returns(foundItem);
        FoundItem actual = _foundItemService.GetFoundItemsById(1);
        Assert.That(actual, Is.EqualTo(foundItem));
    }
    [Test]
    public void DeleteFoundItemById_Returns_True_When_Item_Deleted()
    {
        _foundItemRepoMock.Setup(r => r.DeleteFoundItemById(1)).Returns(true);
        bool actual = _foundItemService.DeleteFoundItemById(1);
        Assert.That(actual, Is.True);
    }
    [Test]
    public void DeleteFoundItem_Returns_False()
    {
        _foundItemRepoMock.Setup(r => r.DeleteFoundItemById(1)).Returns(false);

        bool actual = _foundItemService.DeleteFoundItemById(1);

        Assert.That(actual, Is.False);
    }
        [Test]
        public void UpdateFoundItemById_CallsRepositoryOnce()
        {
            // Arrange
            var dto = new UpdateFoundItemDTO(
                "London",
                "SW1A1AA",
                "john.doe@example.com",
                "07123456789",
                "Wallet",
                "Black leather wallet",
                "Found near station",
                "wallet.jpg"
            );

            var foundItem = new FoundItem(
                "London",
                "SW1A1AA",
                "john.doe@example.com",
                "07123456789",
                "Wallet",
                "Black leather wallet",
                "Found near station",
                "wallet.jpg",
                "test-user-id"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, 1))
                .Returns(foundItem);

            // Act
            _foundItemService.UpdateFoundItemById(dto, 1);

            // Assert
            _foundItemRepoMock.Verify(
                r => r.UpdateFoundItemById(dto, 1),
                Times.Once);
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
                "Black leather wallet",
                "Found near station",
                "wallet.jpg"
            );

            var expected = new FoundItem(
                "London",
                "SW1A1AA",
                "john.doe@example.com",
                "07123456789",
                "Wallet",
                "Black leather wallet",
                "Found near station",
                "wallet.jpg",
                "test-user-id"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, 1))
                .Returns(expected);

            // Act
            var result = _foundItemService.UpdateFoundItemById(dto, 1);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UpdateFoundItemById_ReturnsNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            var dto = new UpdateFoundItemDTO(
                "London",
                "SW1A1AA",
                "john.doe@example.com",
                "07123456789",
                "Wallet",
                "Black leather wallet",
                "Found near station",
                "wallet.jpg"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, 1))
                .Returns((FoundItem)null);

            // Act
            var result = _foundItemService.UpdateFoundItemById(dto, 1);

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public void UpdateFoundItemById_PassesCorrectArgumentsToRepository()
        {
            // Arrange
            var dto = new UpdateFoundItemDTO(
                "Manchester",
                "M11AA",
                "test@test.com",
                "07123456789",
                "Phone",
                "iPhone 15",
                "Blue case",
                "phone.jpg"
            );

            var foundItem = new FoundItem(
                "Manchester",
                "M11AA",
                "test@test.com",
                "07123456789",
                "Phone",
                "iPhone 15",
                "Blue case",
                "phone.jpg",
                "test-user-id"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, 5))
                .Returns(foundItem);

            // Act
            _foundItemService.UpdateFoundItemById(dto, 5);

            // Assert
            _foundItemRepoMock.Verify(r =>
                r.UpdateFoundItemById(
                    It.Is<UpdateFoundItemDTO>(x =>
                        x.City == "Manchester" &&
                        x.Category == "Phone"),
                    5),
                Times.Once);
        }

        [Test]
        public void UpdateFoundItemById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var dto = new UpdateFoundItemDTO(
                "London",
                "SW1A1AA",
                "john@test.com",
                "07123456789",
                "Wallet",
                "Wallet",
                "Extra info",
                "wallet.jpg"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, -1))
                .Returns((FoundItem)null);

            // Act
            var result = _foundItemService.UpdateFoundItemById(dto, -1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateFoundItemById_WhenRepositoryThrows_ExceptionIsThrown()
        {
            // Arrange
            var dto = new UpdateFoundItemDTO(
                "London",
                "SW1A1AA",
                "john@test.com",
                "07123456789",
                "Wallet",
                "Wallet",
                "Extra info",
                "wallet.jpg"
            );

            _foundItemRepoMock
                .Setup(r => r.UpdateFoundItemById(dto, 1))
                .Throws(new Exception("Database failure"));

            // Act & Assert
            Assert.Throws<Exception>(() =>
                _foundItemService.UpdateFoundItemById(dto, 1));
        }

    public void AddOneFoundItem_Returns_Ok_With_Added_Item()
    {
        FoundItem addFoundItem = new FoundItem();
        _foundItemRepoMock.Setup(a => a.AddOneFoundItem(addFoundItem)).Returns(addFoundItem);
        FoundItem actual = _foundItemService.AddOneFoundItem(addFoundItem);
        Assert.That(actual, Is.EqualTo(addFoundItem));
    }
}
