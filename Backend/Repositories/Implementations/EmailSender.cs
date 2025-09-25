using Backend.Repositories.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace Backend.Repositories.Implementations;

public class EmailSender:IEmailSender
{
    public async Task SendEmailAsync(string name, string email, string subject, string message)
    {
        var messageBody = new MimeMessage();
        
        messageBody.From.Add(
            new MailboxAddress(
                "Personal-Knowledge Management App(no-reply)","tourismcentricsocialnetworking@gmail.com"
                )
            );
        
        messageBody.To.Add(new MailboxAddress(name, email));
        messageBody.Subject = subject;
        messageBody.Body = new TextPart(MimeKit.Text.TextFormat.Html);
        
        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587,MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("tourismcentricsocialnetworking@gmail.com", "hsqu iaip dskr nijd");
        await client.SendAsync(messageBody);
        await client.DisconnectAsync(true);
    }
}