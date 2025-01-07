using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Domain.Enums;
using WebAPI.Controllers;

public class AppointmentControllerTests
{
    private readonly Mock<IAppointmentService> _mockAppointmentService;
    private readonly AppointmentController _controller;

    public AppointmentControllerTests()
    {
        _mockAppointmentService = new Mock<IAppointmentService>();
        _controller = new AppointmentController(_mockAppointmentService.Object);
    }

    [Fact]
    public void Create_ReturnsOk_WhenAppointmentIsCreated()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var appointmentDate = DateTime.UtcNow.AddDays(1); // Future appointment
        var newAppointment = new Appointment(doctorId, patientId, appointmentDate, "Test notes");

        _mockAppointmentService
            .Setup(service => service.CreateAppointment(It.IsAny<Appointment>()))
            .Returns(newAppointment);

        // Act
        var result = _controller.Create(newAppointment);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAppointment = Assert.IsType<Appointment>(okResult.Value);
        Assert.Equal(newAppointment.Id, returnedAppointment.Id);
        Assert.Equal(newAppointment.Status, returnedAppointment.Status);
    }

    [Fact]
    public void Create_ReturnsBadRequest_WhenAppointmentValidationFails()
    {
        // Arrange: Appointment with invalid DoctorId
        var newAppointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Test notes");
        newAppointment.DoctorId = Guid.Empty;
        // Act
        var result = _controller.Create(newAppointment);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("DoctorId cannot be empty.", badRequestResult.Value);
    }

    [Fact]
    public void Create_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var newAppointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Test notes");

        _mockAppointmentService
            .Setup(service => service.CreateAppointment(It.IsAny<Appointment>()))
            .Throws(new Exception("Error creating appointment"));

        // Act
        var result = _controller.Create(newAppointment);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error creating appointment", badRequestResult.Value);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenAppointmentIsUpdated()
    {
        // Arrange
        var updateAppointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Updated notes");

        _mockAppointmentService
            .Setup(service => service.UpdateAppointment(It.IsAny<Appointment>()));

        // Act
        var result = _controller.Update(updateAppointment);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var updateAppointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Updated notes");

        _mockAppointmentService
            .Setup(service => service.UpdateAppointment(It.IsAny<Appointment>()))
            .Throws(new Exception("Error updating appointment"));

        // Act
        var result = _controller.Update(updateAppointment);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error updating appointment", badRequestResult.Value);
    }

    [Fact]
    public void Get_ReturnsOk_WhenAppointmentIsFound()
    {
        // Arrange

        var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), "Test appointment");

        _mockAppointmentService
            .Setup(service => service.GetAppointmentById(appointment.Id))
            .Returns(appointment);

        // Act
        var result = _controller.Get(appointment.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAppointment = Assert.IsType<Appointment>(okResult.Value);
        Assert.Equal(appointment.Id, returnedAppointment.Id);
    }

    [Fact]
    public void Get_ReturnsNotFound_WhenAppointmentIsNotFound()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        _mockAppointmentService
            .Setup(service => service.GetAppointmentById(It.IsAny<Guid>()))
            .Throws(new Exception("Appointment not found"));

        // Act
        var result = _controller.Get(appointmentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Appointment not found", notFoundResult.Value);
    }
}
