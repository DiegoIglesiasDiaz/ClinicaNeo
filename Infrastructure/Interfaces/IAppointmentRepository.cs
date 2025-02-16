namespace Infrastructure.Interfaces;
using Domain.Models;
using Infrastructure.Migrations;

public interface IAppointmentRepository
{
    Appointment Add(Appointment Appointment);
    void Update(Appointment Appointment);
    Appointment? GetById(int id);
    List<Appointment>? GetAll();
    Task<bool> IsAppointmentAvailableAsync(Appointment appointment);
    Task<List<DateTime>> GetBookedDatesAsync();
}
