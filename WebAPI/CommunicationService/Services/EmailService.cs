using Domain.Models;
using MailKit.Security;
using MimeKit;
using WebAPI.CommunicationService.Interfaces;

namespace WebAPI.CommunicationService.Services
{
    public class EmailService : IEmailService
    {
        private const string SmtpServer = "smtp.ionos.es";
        private const int SmtpPort = 587;
        private readonly string _username;
        private readonly string _password;

        public EmailService(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task SendEmailAsync(Email email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Centro de Técnicas Naturales Neo", _username));
            message.To.Add(new MailboxAddress("", email.ToEmail));
            message.Subject = email.Subject;

            var bodyBuilder = new BodyBuilder { TextBody = email.IsHtml ? null : email.Body, HtmlBody = email.IsHtml ? email.Body : null };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_username, _password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
