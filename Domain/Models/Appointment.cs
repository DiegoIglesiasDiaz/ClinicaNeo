using Domain.Enums;
using System;

public class Appointment
{
    public Guid Id { get; set; } 
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime AppointmentDate { get; set; } 
    public AppointmentStatus Status { get; set; } 
    public string? Notes { get; set; }

    public Appointment(Guid doctorId, Guid patientId, DateTime appointmentDate, string? notes)
    {
        Id = Guid.NewGuid();
        DoctorId = doctorId;
        PatientId = patientId;
        AppointmentDate = appointmentDate;
        Status = AppointmentStatus.Scheduled;
        Notes = notes;
        Validate();
    }
    public void Validate()
    {
        if (DoctorId == Guid.Empty)
            throw new ArgumentException("DoctorId cannot be empty.");

        if (PatientId == Guid.Empty)
            throw new ArgumentException("PatientId cannot be empty.");

        if (DoctorId == PatientId)
            throw new ArgumentException("DoctorId and PatientId cannot be the same.");

        if (AppointmentDate < DateTime.Now)
            throw new ArgumentException("AppointmentDate cannot be in the past.");

    }
}