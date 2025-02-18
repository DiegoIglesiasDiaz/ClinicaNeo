using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.CommunicationService.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _EmailService;

    public EmailController(IEmailService emailService)
    {
        _EmailService = emailService;
    }

    [HttpPost]
    public IActionResult SendEmail([FromBody] Email email)
    {
        try
        {
            _EmailService.SendEmailAsync(email);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
