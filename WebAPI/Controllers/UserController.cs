using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userService;

    public UserController(IUserRepository userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] User newUser)
    {
        try
        {
            newUser.Validate();
            var entity = _userService.Add(newUser);
            return Ok(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] User updateUser)
    {
        try
        {
            _userService.Update(updateUser);
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
            var user = _userService.GetById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
