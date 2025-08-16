using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents a refresh token for JWT authentication
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// The ID of the user this refresh token belongs to
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The refresh token value
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the JWT token this refresh token is associated with
    /// </summary>
    [Required]
    [StringLength(255)]
    public string JwtId { get; set; } = string.Empty;

    /// <summary>
    /// Whether this refresh token has been used
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// Whether this refresh token has been revoked
    /// </summary>
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// When this refresh token expires
    /// </summary>
    public DateTimeOffset ExpiryDate { get; set; }

    /// <summary>
    /// When this refresh token was revoked (if applicable)
    /// </summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>
    /// The IP address that revoked this token (if applicable)
    /// </summary>
    [StringLength(45)]
    public string? RevokedByIp { get; set; }

    /// <summary>
    /// The token that replaced this one (for token rotation)
    /// </summary>
    [StringLength(255)]
    public string? ReplacedByToken { get; set; }

    // Navigation properties
    /// <summary>
    /// The user this refresh token belongs to
    /// </summary>
    public virtual User User { get; set; } = null!;
}
