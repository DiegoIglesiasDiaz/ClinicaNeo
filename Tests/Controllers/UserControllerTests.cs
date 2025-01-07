using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Domain.Models;
namespace Tests.Controllers;
public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public void Create_ReturnsOkResult_WhenUserIsCreated()
    {
        // Arrange
        var user = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        _mockUserService
            .Setup(service => service.CreateUser(user.FirstName, user.LastName, user.Email))
            .Returns(user);

        // Act
        var result = _controller.Create(user);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user.FirstName, returnValue.FirstName);
        Assert.Equal(user.LastName, returnValue.LastName);
        Assert.Equal(user.Email, returnValue.Email);
        Assert.Equal(user.Id, returnValue.Id);
    }

    [Fact]
    public void Create_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var newUser = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");

        _mockUserService
            .Setup(service => service.CreateUser(newUser.FirstName, newUser.LastName, newUser.Email))
            .Throws(new Exception());

        // Act
        var result = _controller.Create(newUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenUserIsUpdated()
    {
        // Arrange
        var updateUser = new User("Diego", "Iglesias", "Diego.Iglesias@example.com"); 

        _mockUserService
            .Setup(service => service.UpdateUser(updateUser.Id, updateUser.FirstName, updateUser.LastName, updateUser.Email))
            .Verifiable();

        // Act
        var result = _controller.Update(updateUser.Id, updateUser); 

        // Assert
        Assert.IsType<NoContentResult>(result);  
        _mockUserService.Verify(); 
    }


    [Fact]
    public void Update_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var updateUser = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");
        var id = updateUser.Id; 

  
        _mockUserService
            .Setup(service => service.UpdateUser(id, updateUser.FirstName, updateUser.LastName, updateUser.Email))
            .Throws(new Exception());

        // Act
        var result = _controller.Update(id, updateUser); 

        // Assert
        Assert.IsType<BadRequestObjectResult>(result); 

    }

    [Fact]
    public void Get_ReturnsOkResult_WhenUserIsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new User("Diego", "Iglesias", "Diego.Iglesias@example.com");
        _mockUserService
            .Setup(service => service.GetUserById(userId))
            .Returns(expectedUser);

        // Act
        var result = _controller.Get(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<User>(okResult.Value);
        Assert.Equal(expectedUser.Id, returnValue.Id);
        Assert.Equal(expectedUser.FirstName, returnValue.FirstName);
        Assert.Equal(expectedUser.LastName, returnValue.LastName);
    }

    [Fact]
    public void Get_ReturnsNotFound_WhenUserIsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mockUserService
            .Setup(service => service.GetUserById(userId))
            .Throws(new Exception());

        // Act
        var result = _controller.Get(userId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
