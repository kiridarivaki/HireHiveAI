using HireHive.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HireHive.Infrastructure.Services;

public class EmailService(EmailSettings emailSettings, ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings;
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmail(string toEmail, string subject, string plainTextMessage, string? htmlMessage = null)
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
        var subject = "HireHive: Email address confirmation";
        var plainTextMessage = $"Copy this token and add it to the confirmation page to confirm your email address: {token}";

        await SendEmail(toEmail, subject, plainTextMessage, null);
    }
}
