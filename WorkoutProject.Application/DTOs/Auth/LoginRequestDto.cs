using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Application.DTOs.Auth;

/// <summary>
/// DTO for user login request
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Username or email
    /// </summary>
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User password
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether to remember the user for longer period
    /// </summary>
    public bool RememberMe { get; set; } = false;
}
