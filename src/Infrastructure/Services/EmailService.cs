using HireHive.Application.Interfaces;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HireHive.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }

        public async Task SendEmail(string toEmail, string subject, string plainTextMessage, string htmlMessage)
        {
            try
            {
                var client = new SendGridClient(_emailSettings.ApiKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_emailSettings.FromEmail, _emailSettings.Name),
                    Subject = subject,
                    PlainTextContent = plainTextMessage,
                    HtmlContent = htmlMessage
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

        public async Task SendConfirmationEmail(string toEmail, string token)
        {
            var confirmationUrl = $"https://localhost:4700/confirm-email?token={Uri.EscapeDataString(token)}";

            var subject = "HireHive: Email address confirmation";
            var plainTextMessage = "Click the link below to confirm your email address:";
            var htmlMessage = $"<br /><a href=\"{confirmationUrl}\">Confirm Email</a>";

            await SendEmail(toEmail, subject, plainTextMessage, htmlMessage);
        }

        public Task SendPasswordResetEmailAsync(string toEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
