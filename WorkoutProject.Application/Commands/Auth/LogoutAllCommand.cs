using MediatR;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for logging out user from all devices
/// </summary>
public class LogoutAllCommand : IRequest<bool>
{
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
