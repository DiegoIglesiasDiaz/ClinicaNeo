using Application.Interfaces;
using Infrastructure.Interfaces;
using Domain.Models;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _AppointmentRepository;

    public AppointmentService(IAppointmentRepository AppointmentRepository)
    {
        _AppointmentRepository = AppointmentRepository;
    }

    public Appointment CreateAppointment(Appointment Appointment)
    {
        _AppointmentRepository.Add(Appointment);
        return Appointment;
    }

    public void UpdateAppointment(Appointment Appointment)
    {
        if (_AppointmentRepository.GetById(Appointment.Id) == null)
        {
            throw new KeyNotFoundException("Appointment not found");
        }
        _AppointmentRepository.Update(Appointment);

    }

    public Appointment GetAppointmentById(Guid id)
    {
        var Appointment = _AppointmentRepository.GetById(id);
        if (Appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found");
        }
        return Appointment;
    }
}