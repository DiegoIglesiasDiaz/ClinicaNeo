namespace Infrastructure.Interfaces;
using Domain.Models;
public interface IAppointmentRepository
{
    Appointment Add(Appointment Appointment);
    void Update(Appointment Appointment);
    Appointment? GetById(int id);
    Task<bool> IsAppointmentAvailableAsync(Appointment appointment);
}
