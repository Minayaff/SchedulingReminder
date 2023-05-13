
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace SchedulingReminders.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromEmail"]));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), true);
            var x = emailSettings["SmtpUsername"];
            await client.AuthenticateAsync(emailSettings["FromEmail"], emailSettings["SmtpPassword"]);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }

    }


}
