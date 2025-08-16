using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Application.DTOs.Auth;

/// <summary>
/// DTO for forgot password request
/// </summary>
public class ForgotPasswordRequestDto
{
    /// <summary>
    /// Email address to send reset link to
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;
}
