using Microsoft.AspNetCore.Identity.UI.Services;

namespace HireHive.Application.Interfaces
{
    public interface IEmailService : IEmailSender
    {
        Task SendEmailConfirmationAsync(string toEmail, Guid userId, string token);
        Task SendPasswordResetEmailAsync(string toEmail, string subject, string message);
    }
}
