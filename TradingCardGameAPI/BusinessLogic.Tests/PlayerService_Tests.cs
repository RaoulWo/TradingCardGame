using BusinessObjects.Entities;
using BusinessObjects.Interfaces.Services;
using Moq;

namespace BusinessLogic.Tests;

public class PlayerService_Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Username_should_be_available()
    {
        // Arrange
        var mockedPlayerService = new Mock<IPlayerService>();

        mockedPlayerService.Setup(m => m.CheckIfUsernameIsAvailable("AvailableName")).Returns(true);
        mockedPlayerService.Setup(m => m.CheckIfUsernameIsAvailable("UnavailableName")).Returns(false);

        var username = "AvailableName";

        // Act
        var result = mockedPlayerService.Object.CheckIfUsernameIsAvailable(username);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Username_should_be_unavailable()
    {
        // Arrange
        var mockedPlayerService = new Mock<IPlayerService>();

        mockedPlayerService.Setup(m => m.CheckIfUsernameIsAvailable("AvailableName")).Returns(true);
        mockedPlayerService.Setup(m => m.CheckIfUsernameIsAvailable("UnavailableName")).Returns(false);

        var username = "UnavailableName";

        // Act
        var result = mockedPlayerService.Object.CheckIfUsernameIsAvailable(username);

        // Assert
        Assert.IsFalse(result);
    }
}