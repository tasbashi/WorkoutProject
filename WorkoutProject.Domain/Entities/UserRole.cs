using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between Users and Roles
/// </summary>
public class UserRole
{
    /// <summary>
    /// The ID of the user
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// The ID of the role
    /// </summary>
    [Required]
    public Guid RoleId { get; set; }

    /// <summary>
    /// When the role was assigned to the user
    /// </summary>
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// The ID of the user who assigned this role (optional)
    /// </summary>
    public Guid? AssignedBy { get; set; }

    /// <summary>
    /// Whether this role assignment is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    /// <summary>
    /// The user this role is assigned to
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// The role being assigned
    /// </summary>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// The user who assigned this role (optional)
    /// </summary>
    public virtual User? AssignedByUser { get; set; }

    // Remove the private constructor and other methods that might cause conflicts
    // Keep the entity simple as it's a junction table
}