using BusinessLogic.Services;

namespace BusinessLogic.Tests;

public class SessionService_Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Hashed_password_should_have_length_48()
    {
        // Arrange
        string pwd = "Password";

        // Act
        string hashedPwd = SessionService.Instance.Hash(pwd);
        int length = hashedPwd.Length;

        var expectedLength = 48;

        // Assert
        Assert.AreEqual(length, expectedLength);
    }

    [Test]
    public void Password_verification_should_succeed()
    {
        // Arrange
        var enteredPwd = "Raoul";
        var storedPwd = "spgFLJWV0aRWbi55dZb2SGonss3yyNZh+rjAD2sYoyIkuS4f";

        // Act
        var result = SessionService.Instance.VerifyPassword(enteredPwd, storedPwd);
        
        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Password_verification_should_fail()
    {
        // Arrange
        var enteredPwd = "raoul";
        var storedPwd = "spgFLJWV0aRWbi55dZb2SGonss3yyNZh+rjAD2sYoyIkuS4f";

        // Act
        var result = SessionService.Instance.VerifyPassword(enteredPwd, storedPwd);

        // Assert
        Assert.IsFalse(result);
    }

}