using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace WebAPI.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]


public class UserController : ControllerBase
{
    private readonly IUserRepository _userService;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(IConfiguration configuration, IUserRepository userService, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        try
        {
            // Attempt to sign in the user
            var signInResult = await _signInManager.PasswordSignInAsync(user.Username, user.Password, true, false);

            if (signInResult.Succeeded)
            {
                // Find the user
                var appUser = await _userManager.FindByNameAsync(user.Username);
                if (appUser == null) return BadRequest("Invalid user");

                // Generate the JWT token
                var token = await GenerateJwtToken(appUser);

                // Return the token
                return Ok(new { Token = token, Username = appUser.UserName });
            }
            else
            {
                return BadRequest("Invalid username or password");
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Error: " + ex.Message);
        }
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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
