using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NonWorkingDayRepositoryController : ControllerBase
{
    private readonly INonWorkingDayRepository _NonWorkingDayService;

    public NonWorkingDayRepositoryController(INonWorkingDayRepository NonWorkingDayRepositoryService)
    {
       _NonWorkingDayService = NonWorkingDayRepositoryService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] NonWorkingDay newNonWorkingDay)
    {
        try
        {
            newNonWorkingDay.Validate();
            var entity = _NonWorkingDayService.Add(newNonWorkingDay);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] NonWorkingDay updateNonWorkingDay)
    {
        try
        {
            updateNonWorkingDay.Validate();
            _NonWorkingDayService.Update(updateNonWorkingDay);
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
            var NonWorkingDayRepository =_NonWorkingDayService.GetAll();
            return Ok(NonWorkingDayRepository);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("Date")]
    public IActionResult GetByDate([FromBody] DateTime date)
    {
        try
        {
            var NonWorkingDayRepository = _NonWorkingDayService.GetByDate(date);
            return Ok(NonWorkingDayRepository);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
