using MailKit.Net.Smtp;
using MimeKit;

namespace SchoolApiService.Services;

public class EmailService(IConfiguration configuration)
{

    public async Task SendEmailAsync(string toEmail, string subject, string userName, string link)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "email_template.html");
        var templateContent = await File.ReadAllTextAsync(templatePath);

        // Replace the placeholder with the actual body content
        var emailContent = templateContent.Replace("{{userName}}", userName).Replace("{{verificationLink}}", link);
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(configuration["EmailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = emailContent };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(configuration["EmailSettings:Host"], int.Parse(configuration["EmailSettings:Port"] ?? "465"), true);
        await smtp.AuthenticateAsync(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}