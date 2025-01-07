using Domain.Models;
namespace Tests.Models;
public class UserModelTests
{
    [Fact]
    public void Update_ValidData_UpdatesUserDetails()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        // Act
        user.Update("Hello", "World", "Hello.World@example.com");

        // Assert
        Assert.Equal("Hello", user.FirstName);
        Assert.Equal("World", user.LastName);
        Assert.Equal("Hello.World@example.com", user.Email);
    }

    [Fact]
    public void Update_EmptyFirstName_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.Update("", "Iglesias", "Diego.Iglesias@example.com"));
    }

    [Fact]
    public void Update_EmptyLastName_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => user.Update("Diego", "", "Diego.Iglesias@example.com"));
        Assert.Equal("Last name cannot be empty", exception.Message);
    }

    [Fact]
    public void Update_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => user.Update("Diego", "Iglesias", "invalid-email"));
        Assert.Equal("Email is invalid", exception.Message);
    }

    [Fact]
    public void Update_EmptyEmail_ThrowsArgumentException()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => user.Update("Diego", "Iglesias", ""));
        Assert.Equal("Email is invalid", exception.Message);
    }
}
