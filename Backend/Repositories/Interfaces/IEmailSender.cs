namespace Backend.Repositories.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string name, string email, string subject, string message);
}