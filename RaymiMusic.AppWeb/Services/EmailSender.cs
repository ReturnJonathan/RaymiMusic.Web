using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit.Text;      // para TextFormat
using System.Threading.Tasks;


namespace RaymiMusic.AppWeb.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _config;
        public EmailSender(IOptions<EmailConfiguration> config)
        {
            _config = config.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // 1) Crear mensaje
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config.From));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            // 2) Enviar vía SMTP
            using var client = new SmtpClient();
            await client.ConnectAsync(_config.SmtpServer, _config.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_config.Username, _config.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
