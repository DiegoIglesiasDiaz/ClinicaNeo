using Domain.Enums;
using Domain.Models;
using System;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public Patient Patient { get; set; }
    public DateTime StartDateTime => Date.Date + StartTime;
    public DateTime EndDateTime => Date.Date + EndTime;

    public Appointment()
    {
        Patient = new Patient();
    }
    public Appointment(DateTime date, Schedule schedule, Patient patient, AppointmentStatus status, string? notes)
    {
        Date = date;
        StartTime = schedule.StartTime;
        EndTime = schedule.EndTime;
        Status = status;
        Patient = patient;
        Notes = notes;
    }
    public void Validate()
    {
        if (PatientId == null)
            throw new ArgumentException("PatientId cannot be empty.");
        if (Date.Date < DateTime.Now.Date)
            throw new ArgumentException("Date cannot be in the past.");
        if (StartTime == null)
            throw new ArgumentException("Must declare a Start Time");
        if (EndTime == null)
            throw new ArgumentException("Must declare an End Time");
        if (Status == null)
            throw new ArgumentException("Must declare Status");
    }
}
