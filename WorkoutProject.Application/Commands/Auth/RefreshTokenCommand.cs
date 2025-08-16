using MediatR;
using WorkoutProject.Application.DTOs.Auth;

namespace WorkoutProject.Application.Commands.Auth;

/// <summary>
/// Command for refreshing access token using refresh token
/// </summary>
public class RefreshTokenCommand : IRequest<LoginResponseDto>
{
    /// <summary>
    /// The refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// The expired access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// IP address of the client
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client
    /// </summary>
    public string? UserAgent { get; set; }
}
