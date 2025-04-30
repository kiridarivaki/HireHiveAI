using HireHive.Application.Interfaces;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HireHive.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _name;
        private readonly ILogger<EmailService> _logger;
        public EmailService(string apiKey, string fromEmail, string name, ILogger<EmailService> logger)
        {
            _apiKey = apiKey;
            _fromEmail = fromEmail;
            _name = name;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var client = new SendGridClient(_apiKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_fromEmail, _name),
                    Subject = subject,
                    PlainTextContent = message,
                    HtmlContent = message
                };

                msg.AddTo(new EmailAddress(toEmail));

                msg.SetClickTracking(false, false);

                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Sendgrid failed to send email.");

                _logger.LogInformation("Email to {toEmail} queued successfully.", toEmail);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to send email to {toEmail}. With exception: {message}", toEmail, e.Message);
                throw;
            }
        }

        public async Task SendEmailConfirmationAsync(string toEmail, Guid userId, string token)
        {
            var confirmationUrl = $"https://localhost:4700/confirm-email?userId={Uri.EscapeDataString(userId.ToString())}&token={Uri.EscapeDataString(token)}";

            var subject = "HireHive: Email address confirmation";
            var message = $"Click the link below to confirm your email address:<br /><a href=\"{confirmationUrl}\">Confirm Email</a>";

            await SendEmailAsync(toEmail, subject, message);
        }

        public Task SendPasswordResetEmailAsync(string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
