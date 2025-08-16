using MediatR;
using WorkoutProject.Application.DTOs.Auth;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for user login
/// </summary>
public class LoginCommand : IRequest<LoginResponseDto>
{
    /// <summary>
    /// Username or email
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Remember me flag
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// IP address of the client
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client
    /// </summary>
    public string? UserAgent { get; set; }
}
