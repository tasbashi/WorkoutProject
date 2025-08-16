using WorkoutProject.Domain.Enums;

namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents an athlete in the fitness tracking system
/// </summary>
public class Athlete : BaseEntity
{
    /// <summary>
    /// Reference to the User entity
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Reference to the assigned trainer (optional)
    /// </summary>
    public Guid? TrainerId { get; set; }
    
    /// <summary>
    /// Height in centimeters
    /// </summary>
    public decimal? Height { get; set; }
    
    /// <summary>
    /// Weight in kilograms
    /// </summary>
    public decimal? Weight { get; set; }
    
    /// <summary>
    /// Activity level of the athlete
    /// </summary>
    public ActivityLevel? ActivityLevel { get; set; }
    
    /// <summary>
    /// Fitness goals as JSON array
    /// </summary>
    public string? Goals { get; set; }
    
    /// <summary>
    /// Medical history information
    /// </summary>
    public string? MedicalHistory { get; set; }
    
    /// <summary>
    /// Known allergies
    /// </summary>
    public string? Allergies { get; set; }
    
    /// <summary>
    /// Emergency contact name
    /// </summary>
    public string? EmergencyContactName { get; set; }
    
    /// <summary>
    /// Emergency contact phone number
    /// </summary>
    public string? EmergencyContactPhone { get; set; }
    
    /// <summary>
    /// Emergency contact relationship
    /// </summary>
    public string? EmergencyContactRelationship { get; set; }
    
    /// <summary>
    /// When the athlete joined the trainer
    /// </summary>
    public DateTimeOffset? JoinedTrainerAt { get; set; }
    
    /// <summary>
    /// Whether the athlete is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    /// <summary>
    /// The associated user
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// The assigned trainer (if any)
    /// </summary>
    public virtual Trainer? Trainer { get; set; }
    
    // Computed properties
    /// <summary>
    /// Calculate BMI if height and weight are available
    /// </summary>
    public decimal? BMI
    {
        get
        {
            if (Height.HasValue && Weight.HasValue && Height > 0)
            {
                var heightInMeters = Height.Value / 100;
                return Math.Round(Weight.Value / (heightInMeters * heightInMeters), 2);
            }
            return null;
        }
    }
}
