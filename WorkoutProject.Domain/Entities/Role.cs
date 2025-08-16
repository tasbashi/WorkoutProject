using System.ComponentModel.DataAnnotations;

namespace WorkoutProject.Domain.Entities;

public class Role : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string NormalizedName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string? Permissions { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}