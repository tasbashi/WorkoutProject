using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents a claim that a user possesses
/// </summary>
public class UserClaim
{
    /// <summary>
    /// The primary key for this claim
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user this claim belongs to
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The claim type
    /// </summary>
    [Required]
    [StringLength(255)]
    public string ClaimType { get; set; } = string.Empty;

    /// <summary>
    /// The claim value
    /// </summary>
    [StringLength(255)]
    public string? ClaimValue { get; set; }

    /// <summary>
    /// When this claim was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation properties
    /// <summary>
    /// The user this claim belongs to
    /// </summary>
    public virtual User User { get; set; } = null!;
}
