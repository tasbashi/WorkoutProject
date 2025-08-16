using MediatR;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for forgot password request
/// </summary>
public class ForgotPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Email address to send reset link
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Base URL for reset link
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
}
