using MediatR;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for resetting password
/// </summary>
public class ResetPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password reset token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
