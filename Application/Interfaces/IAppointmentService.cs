using Domain.Enums;
using Domain.Models;
namespace Application.Interfaces;

public interface IAppointmentService
{
    Appointment CreateAppointment(Appointment Appointment);
    void UpdateAppointment(Appointment Appointment);
    Appointment GetAppointmentById(Guid id);
}