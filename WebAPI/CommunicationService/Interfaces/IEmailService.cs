using Domain.Models;

namespace WebAPI.CommunicationService.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
    }
}
