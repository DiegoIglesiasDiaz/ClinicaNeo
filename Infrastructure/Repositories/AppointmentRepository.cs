using Domain.Models;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicaNeoContext _context;

    public AppointmentRepository(ClinicaNeoContext context)
    {
        _context = context;
    }

    public void Add(Appointment Appointment)
    {
        _context.Appointments.Add(Appointment);
        _context.SaveChanges();
    }

    public void Update(Appointment Appointment)
    {
        _context.Appointments.Update(Appointment);
        _context.SaveChanges();
    }

    public Appointment? GetById(Guid id)
    {
        return _context.Appointments.SingleOrDefault(u => u.Id == id);
    }
}