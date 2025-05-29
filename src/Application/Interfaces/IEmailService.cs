namespace HireHive.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string plainTextMessage, string? htmlMessage = null);
        Task SendConfirmationEmail(string toEmail, string token);
    }
}
