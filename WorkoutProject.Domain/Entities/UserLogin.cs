using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents a login (external provider) for a user
/// </summary>
public class UserLogin
{
    /// <summary>
    /// The login provider (e.g., Google, Facebook, Microsoft)
    /// </summary>
    [Required]
    [StringLength(128)]
    public string LoginProvider { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier for the user from the provider
    /// </summary>
    [Required]
    [StringLength(128)]
    public string ProviderKey { get; set; } = string.Empty;

    /// <summary>
    /// The display name for the provider
    /// </summary>
    [StringLength(255)]
    public string? ProviderDisplayName { get; set; }

    /// <summary>
    /// The ID of the user this login belongs to
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// When this login was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation properties
    /// <summary>
    /// The user this login belongs to
    /// </summary>
    public virtual User User { get; set; } = null!;
}
