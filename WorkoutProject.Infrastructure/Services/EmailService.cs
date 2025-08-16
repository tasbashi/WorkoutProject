using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkoutProject.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace WorkoutProject.Infrastructure.Services;

/// <summary>
/// Service for sending emails
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpHost = emailSettings["SmtpHost"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"]!);
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];
            var fromEmail = emailSettings["FromEmail"];
            var fromName = emailSettings["FromName"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {Email}", to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", to);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetEmailAsync(string to, string resetLink, string firstName)
    {
        var subject = "Reset Your Password";
        var body = $@"
            <html>
            <body>
                <h2>Password Reset Request</h2>
                <p>Hi {firstName},</p>
                <p>We received a request to reset your password. Click the link below to reset it:</p>
                <p><a href=""{resetLink}"" style=""background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;"">Reset Password</a></p>
                <p>If you didn't request this, please ignore this email.</p>
                <p>This link will expire in 24 hours.</p>
                <br>
                <p>Best regards,<br>WorkoutProject Team</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string to, string firstName, string? confirmationLink = null)
    {
        var subject = "Welcome to WorkoutProject!";
        var confirmationSection = string.IsNullOrEmpty(confirmationLink) 
            ? "" 
            : $@"<p>Please confirm your email address by clicking the link below:</p>
                 <p><a href=""{confirmationLink}"" style=""background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;"">Confirm Email</a></p>";

        var body = $@"
            <html>
            <body>
                <h2>Welcome to WorkoutProject!</h2>
                <p>Hi {firstName},</p>
                <p>Thank you for joining WorkoutProject! We're excited to help you on your fitness journey.</p>
                {confirmationSection}
                <p>If you have any questions, feel free to reach out to our support team.</p>
                <br>
                <p>Best regards,<br>WorkoutProject Team</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, body);
    }
}
