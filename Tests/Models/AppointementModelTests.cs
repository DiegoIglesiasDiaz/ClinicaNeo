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
        var doctorId = Guid.Empty;
        var patientId = Guid.NewGuid();
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment(doctorId, patientId, appointmentDate, "Test notes")
        );
        Assert.Equal("DoctorId cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenPatientIdIsEmpty()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.Empty;
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment(doctorId, patientId, appointmentDate, "Test notes")
        );
        Assert.Equal("PatientId cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDoctorIdAndPatientIdAreTheSame()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = doctorId; // Same ID for doctor and patient
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment(doctorId, patientId, appointmentDate, "Test notes")
        );
        Assert.Equal("DoctorId and PatientId cannot be the same.", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenAppointmentDateIsInThePast()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var appointmentDate = DateTime.Now.AddDays(-1); // Past date

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Appointment(doctorId, patientId, appointmentDate, "Test notes")
        );
        Assert.Equal("AppointmentDate cannot be in the past.", exception.Message);
    }

    [Fact]
    public void Constructor_DoesNotThrowException_WhenAllFieldsAreValid()
    {
        // Arrange
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();
        var appointmentDate = DateTime.Now.AddDays(1); // Future date

        // Act & Assert
        var exception = Record.Exception(() =>
            new Appointment(doctorId, patientId, appointmentDate, "Test notes")
        );
        Assert.Null(exception); // No exception should be thrown
    }
}
