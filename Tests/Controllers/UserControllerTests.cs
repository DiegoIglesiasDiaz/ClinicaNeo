using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
namespace Tests.Controllers;
public class UserControllerTests
{
    private readonly Mock<IUserRepository> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserRepository>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public void Create_ReturnsOkResult_WhenUserIsCreated()
    {
        // Arrange
        var user = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = Role.Admin,
            Address = " C/ Avenida España",
            PhoneNumber = "123-456-7890",

        };
        _mockUserService
        .Setup(service => service.Add(user))
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
        var newUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = Role.Admin,
            Address = " C/ Avenida España",
            PhoneNumber = "123-456-7890",

        };

        _mockUserService
             .Setup(service => service.Add(newUser))
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
        var updateUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = Role.Admin,
            Address = " C/ Avenida España",
            PhoneNumber = "123-456-7890",

        };

        _mockUserService
            .Setup(service => service.Update(updateUser))
            .Verifiable();

        // Act
        var result = _controller.Update(updateUser);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockUserService.Verify();
    }


    [Fact]
    public void Update_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var updateUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = Role.Admin,
            Address = " C/ Avenida España",
            PhoneNumber = "123-456-7890",

        };
        var id = updateUser.Id;


        _mockUserService
            .Setup(service => service.Update(updateUser))
            .Throws(new Exception());

        // Act
        var result = _controller.Update(updateUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);

    }

    [Fact]
    public void Get_ReturnsOkResult_WhenUserIsFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new User
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "User",
            Role = Role.Admin,
            Address = " C/ Avenida España",
            PhoneNumber = "123-456-7890",

        };
        _mockUserService
            .Setup(service => service.GetById(userId))
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
            .Setup(service => service.GetById(userId))
            .Throws(new Exception());

        // Act
        var result = _controller.Get(userId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
