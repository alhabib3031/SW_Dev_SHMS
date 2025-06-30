using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // يمكنك استخدام SMTP أو log فقط للتجريب
        Console.WriteLine($"إرسال بريد إلى: {email} - الموضوع: {subject}");
        return Task.CompletedTask;
    }
}