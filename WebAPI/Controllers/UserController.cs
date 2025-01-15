using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static System.Net.WebRequestMethods;

namespace WebAPI.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]


public class UserController : ControllerBase
{
    private readonly IUserRepository _userService;
    private readonly SignInManager<User> _signInManager;

    public UserController(IUserRepository userService, SignInManager<User> signInManager )
    {
        _userService = userService;
        _signInManager = signInManager;
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        try
        {
            if ((await _signInManager.PasswordSignInAsync(user.Username, user.Password, true, false)).Succeeded)
            {
                return Ok("Login success");
            }
            else
            {
                return BadRequest("Login failed");
            }

        }
        catch (Exception ex)
        {
            return BadRequest("Error: " + ex.Message);
        }
    }





    //[HttpPost]
    //public IActionResult Create([FromBody] User newUser)
    //{
    //    try
    //    {
    //        newUser.Validate();
    //        var entity = _userService.Add(newUser);
    //        return Ok(entity);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpPut]
    //public IActionResult Update([FromBody] User updateUser)
    //{
    //    try
    //    {
    //        _userService.Update(updateUser);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpGet("{id}")]
    //public IActionResult Get(Guid id)
    //{
    //    try
    //    {
    //        var user = _userService.GetById(id);
    //        return Ok(user);
    //    }
    //    catch (Exception ex)
    //    {
    //        return NotFound(ex.Message);
    //    }
    //}
}
