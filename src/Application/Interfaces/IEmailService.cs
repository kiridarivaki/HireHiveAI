namespace HireHive.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage, string plainTextMessage);
        Task SendEmailConfirmationAsync(string toEmail, Guid userId, string token);
        Task SendPasswordResetEmailAsync(string toEmail, string subject, string message);
    }
}
