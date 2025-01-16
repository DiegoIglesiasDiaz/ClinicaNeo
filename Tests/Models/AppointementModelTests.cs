using System;
using Xunit;
using Domain.Models;
using Domain.Enums;
namespace Tests.Models;
public class AppointementModelTests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDoctorIdIsEmpty()
    {
        // Arrange
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment()
        );
        Assert.Equal("DoctorId cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenPatientIdIsEmpty()
    {
        // Arrange
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment()
        );
        Assert.Equal("PatientId cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDoctorIdAndPatientIdAreTheSame()
    {
        // Arrange
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment()
        );
        Assert.Equal("DoctorId and PatientId cannot be the same.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenAppointmentDateIsInThePast()
    {
        // Arrange
        var appointmentDate = DateTime.Now.AddDays(-1); // Past date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment()
        );
        Assert.Equal("Date cannot be in the past.", exception.Message);
    }

    [Fact]
    public void Constructor_DoesNotThrowException_WhenAllFieldsAreValid()
    {
        // Arrange
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Record.Exception(() =>
            new Appointment()
        );
        Assert.Null(exception); // No exception should be thrown
    }
}
