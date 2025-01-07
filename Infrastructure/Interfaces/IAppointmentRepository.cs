namespace Infrastructure.Interfaces;
using Domain.Models;
public interface IAppointmentRepository
{
    void Add(Appointment Appointment);
    void Update(Appointment Appointment);
    Appointment? GetById(Guid id);
}
