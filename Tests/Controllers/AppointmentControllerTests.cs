using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

public class AppointmentControllerTests
{
    private readonly Mock<IAppointmentRepository> _mockAppointmentService;
    private readonly Mock<INonWorkingDayRepository> _mockNonWorkingDayService;
    private readonly Mock<IScheduleRepository> _mockScheduleService;
    private readonly AppointmentController _controller;

    public AppointmentControllerTests()
    {
        _mockAppointmentService = new Mock<IAppointmentRepository>();
        _mockNonWorkingDayService = new Mock<INonWorkingDayRepository>();
        _mockScheduleService = new Mock<IScheduleRepository>();
        _controller = new AppointmentController(_mockAppointmentService.Object, _mockNonWorkingDayService.Object, _mockScheduleService.Object);
    }

    [Fact]
    public void Create_ReturnsOk_WhenAppointmentIsCreated()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var appointmentDate = DateTime.UtcNow.AddDays(1); // Future appointment
        var newAppointment = new Appointment();

        _mockAppointmentService
            .Setup(service => service.Add(It.IsAny<Appointment>()))
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
        var newAppointment = new Appointment();

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
        var newAppointment = new Appointment();

        _mockAppointmentService
            .Setup(service => service.Add(It.IsAny<Appointment>()))
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
        var updateAppointment = new Appointment();

        _mockAppointmentService
            .Setup(service => service.Update(It.IsAny<Appointment>()));

        // Act
        var result = _controller.Update(updateAppointment);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var updateAppointment = new Appointment();

        _mockAppointmentService
            .Setup(service => service.Update(It.IsAny<Appointment>()))
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

        var appointment = new Appointment();

        _mockAppointmentService
            .Setup(service => service.GetById(appointment.Id))
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


        _mockAppointmentService
            .Setup(service => service.GetById(It.IsAny<int>()))
            .Throws(new Exception("Appointment not found"));

        // Act
        var result = _controller.Get(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Appointment not found", notFoundResult.Value);
    }
}
