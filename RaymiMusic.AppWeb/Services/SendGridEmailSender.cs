using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace RaymiMusic.AppWeb.Services
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;
        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_options.ApiKey);
            var from = new EmailAddress(_options.FromEmail, _options.FromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);
            var response = await client.SendEmailAsync(msg);

            // opcional: lanzar si falla
            if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new System.Exception($"Error sending email: {response.StatusCode} - {body}");
            }
        }
    }
}
