namespace WorkoutProject.Application.Interfaces;

/// <summary>
/// Interface for email service
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send email
    /// </summary>
    /// <param name="to">Recipient email</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body (HTML)</param>
    /// <returns>True if sent successfully</returns>
    Task<bool> SendEmailAsync(string to, string subject, string body);

    /// <summary>
    /// Send password reset email
    /// </summary>
    /// <param name="to">Recipient email</param>
    /// <param name="resetLink">Password reset link</param>
    /// <param name="firstName">User's first name</param>
    /// <returns>True if sent successfully</returns>
    Task<bool> SendPasswordResetEmailAsync(string to, string resetLink, string firstName);

    /// <summary>
    /// Send welcome email to new user
    /// </summary>
    /// <param name="to">Recipient email</param>
    /// <param name="firstName">User's first name</param>
    /// <param name="confirmationLink">Email confirmation link</param>
    /// <returns>True if sent successfully</returns>
    Task<bool> SendWelcomeEmailAsync(string to, string firstName, string? confirmationLink = null);
}
