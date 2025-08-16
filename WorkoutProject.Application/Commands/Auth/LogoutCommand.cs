using MediatR;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for user logout
/// </summary>
public class LogoutCommand : IRequest<bool>
{
    /// <summary>
    /// The refresh token to revoke
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// User ID (from JWT)
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// IP address of the client
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client
    /// </summary>
    public string? UserAgent { get; set; }
}
