using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleRepository _ScheduleService;

    public ScheduleController(IScheduleRepository ScheduleService)
    {
        _ScheduleService = ScheduleService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Schedule newSchedule)
    {
        try
        {
            newSchedule.Validate();
            var entity = _ScheduleService.Add(newSchedule);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] Schedule updateSchedule)
    {
        try
        {
            updateSchedule.Validate();
            _ScheduleService.Update(updateSchedule);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var Schedules = _ScheduleService.GetAll();
            return Ok(Schedules);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("GetActiveByDate")]
    public async Task<IActionResult> GetActiveSchedulesByDateAsync(DateTime date)
    {
        try
        {
            var Schedules = await _ScheduleService.GetActiveSchedulesByDateAsync(date);
            return Ok(Schedules);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
