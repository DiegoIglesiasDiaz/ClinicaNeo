using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] User newUser)
    {
        try
        {
            var user = _userService.CreateUser(newUser.FirstName, newUser.LastName, newUser.Email);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] User updateUser)
    {
        try
        {
            _userService.UpdateUser(id, updateUser.FirstName, updateUser.LastName, updateUser.Email);
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
            var user = _userService.GetUserById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
