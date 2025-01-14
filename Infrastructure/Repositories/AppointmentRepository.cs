using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicaNeoContext _context;

    public AppointmentRepository(ClinicaNeoContext context)
    {
        _context = context;
    }

    public Appointment Add(Appointment Appointment)
    {
        var entity = _context.Appointments.Add(Appointment);
        _context.SaveChanges();
        return entity.Entity;
    }

    public void Update(Appointment Appointment)
    {
        _context.Appointments.Update(Appointment);
        _context.SaveChanges();
    }

    public Appointment? GetById(int id)
    {
        return _context.Appointments.SingleOrDefault(u => u.Id == id);
    }

    public async Task<bool> IsAppointmentAvailableAsync(Appointment appointment)
    {
        return  !(await _context.Appointments.AnyAsync(a =>
                   a.Date == appointment.Date &&
                   ((a.StartTime < appointment.EndTime && a.EndTime > appointment.StartTime))));
    }
}