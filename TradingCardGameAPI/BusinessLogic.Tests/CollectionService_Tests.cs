using BusinessLogic.Services;
using BusinessObjects.Entities;

namespace BusinessLogic.Tests;

public class CollectionService_Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Card_should_be_in_collection()
    {
        // Arrange
        CollectionEntity card = new CollectionEntity()
        {
            Id = Guid.Empty,
            FkCardId = Guid.Empty,
            FkPlayerId = Guid.Empty
        };

        List<CollectionEntity> collection = new List<CollectionEntity>();
        collection.Add(card);

        // Act
        bool result = CollectionService.Instance.CheckIfCardIsInCollection(card, collection);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Card_should_not_be_in_collection()
    {
        // Arrange
        CollectionEntity card = new CollectionEntity()
        {
            Id = Guid.Empty,
            FkCardId = Guid.Empty,
            FkPlayerId = Guid.Empty
        };

        CollectionEntity otherCard = new CollectionEntity()
        {
            Id = Guid.NewGuid(),
            FkCardId = Guid.NewGuid(),
            FkPlayerId = Guid.NewGuid()
        };

        List<CollectionEntity> collection = new List<CollectionEntity>();
        collection.Add(otherCard);

        // Act
        bool result = CollectionService.Instance.CheckIfCardIsInCollection(card, collection);

        // Assert
        Assert.IsFalse(result);
    }
}