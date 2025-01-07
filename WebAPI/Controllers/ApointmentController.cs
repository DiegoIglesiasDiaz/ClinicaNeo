using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.Enums;
using System.Net;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _AppointmentService;

    public AppointmentController(IAppointmentService AppointmentService)
    {
        _AppointmentService = AppointmentService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Appointment newAppointment)
    {
        try
        {
            newAppointment.Validate();
            var Appointment = _AppointmentService.CreateAppointment(newAppointment);
            return Ok(Appointment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] Appointment updateAppointment)
    {
        try
        {
            _AppointmentService.UpdateAppointment(updateAppointment);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);  
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        try
        {
            var Appointment = _AppointmentService.GetAppointmentById(id);
            return Ok(Appointment);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
