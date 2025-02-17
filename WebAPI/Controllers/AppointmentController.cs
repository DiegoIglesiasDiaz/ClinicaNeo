using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentRepository _AppointmentService;
    private readonly INonWorkingDayRepository _NonWorkingDayRepository;
    private readonly IScheduleRepository _ScheduleRepository;

    public AppointmentController(IAppointmentRepository AppointmentService, INonWorkingDayRepository NonWorkingDayRepository, IScheduleRepository ScheduleRepository)
    {
        _AppointmentService = AppointmentService;
        _NonWorkingDayRepository = NonWorkingDayRepository;
        _ScheduleRepository = ScheduleRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Appointment newAppointment)
    {
        try
        {
            newAppointment.Validate();
            if (await ValidateAppointmentAsync(newAppointment))
            {
                var entity = _AppointmentService.Add(newAppointment);
                return Ok(entity);
            }
            else
            {
                return BadRequest("This Appointment has been already booked");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Appointment updateAppointment)
    {
        try
        {
            updateAppointment.Validate();
            _AppointmentService.Update(updateAppointment);
            return NoContent();


        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            var Appointment = _AppointmentService.GetById(id);
            return Ok(Appointment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet()]
    public IActionResult GetAll()
    {
        try
        {
            var ListAppointment = _AppointmentService.GetAll();
            return Ok(ListAppointment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("BookedDates")]
    public async Task<IActionResult> GetBookedDatesAsync()
    {
        try
        {
            var Appointment = await _AppointmentService.GetBookedDatesAsync();
            return Ok(Appointment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    private async Task<bool> ValidateAppointmentAsync(Appointment appointment)
    {
        if (await _NonWorkingDayRepository.IsNonWorkingDayAsync(appointment.Date))
            return false;

        var activeSchedules = await _ScheduleRepository.GetActiveSchedulesAsync();
        var isValidTime = activeSchedules.Any(s =>
            s.StartTime == appointment.StartTime && s.EndTime == appointment.EndTime);
        return isValidTime && await _AppointmentService.IsAppointmentAvailableAsync(appointment);

    }
}
