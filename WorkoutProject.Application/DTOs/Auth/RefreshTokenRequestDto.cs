using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Application.DTOs.Auth;

/// <summary>
/// DTO for refresh token request
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// The refresh token
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// The expired access token
    /// </summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}
