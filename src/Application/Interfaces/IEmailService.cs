namespace HireHive.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string htmlMessage, string plainTextMessage);
        Task SendConfirmationEmail(string toEmail, string token);
        Task SendPasswordResetEmailAsync(string toEmail, string subject, string message);
    }
}
