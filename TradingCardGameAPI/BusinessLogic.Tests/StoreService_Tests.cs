using BusinessLogic.Services;

namespace BusinessLogic.Tests;

public class StoreService_Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Package_should_contain_five_cards()
    {
        // Arrange
        var package = StoreService.Instance.GeneratePackage();

        // Act
        var size = package.Count;
        var expectedSize = 5;

        // Assert
        Assert.AreEqual(size, expectedSize);
    }
}