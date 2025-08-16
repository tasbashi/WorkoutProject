namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Represents a trainer in the fitness tracking system
/// </summary>
public class Trainer : BaseEntity
{
    /// <summary>
    /// Reference to the User entity
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Specializations as JSON array
    /// </summary>
    public string? Specializations { get; set; }
    
    /// <summary>
    /// Certifications as JSON array of certification objects
    /// </summary>
    public string? Certifications { get; set; }
    
    /// <summary>
    /// Professional experience description
    /// </summary>
    public string? Experience { get; set; }
    
    /// <summary>
    /// Professional bio/description
    /// </summary>
    public string? Bio { get; set; }
    
    /// <summary>
    /// Hourly rate for training sessions
    /// </summary>
    public decimal? HourlyRate { get; set; }
    
    /// <summary>
    /// Currency for pricing (e.g., USD, EUR)
    /// </summary>
    public string Currency { get; set; } = "USD";
    
    /// <summary>
    /// Whether the trainer is verified by the system
    /// </summary>
    public bool IsVerified { get; set; } = false;
    
    /// <summary>
    /// When the trainer was verified
    /// </summary>
    public DateTimeOffset? VerificationDate { get; set; }
    
    /// <summary>
    /// Average rating from athletes (0.00-5.00)
    /// </summary>
    public decimal? Rating { get; set; }
    
    /// <summary>
    /// Total number of reviews received
    /// </summary>
    public int TotalReviews { get; set; } = 0;
    
    /// <summary>
    /// Maximum number of athletes this trainer can handle
    /// </summary>
    public int? MaxAthletes { get; set; } = 50;
    
    /// <summary>
    /// Whether the trainer is currently accepting new athletes
    /// </summary>
    public bool IsAcceptingNewAthletes { get; set; } = true;
    
    /// <summary>
    /// Trainer's timezone
    /// </summary>
    public string? TimeZone { get; set; }

    // Navigation properties
    /// <summary>
    /// The associated user
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// Athletes assigned to this trainer
    /// </summary>
    public virtual ICollection<Athlete> Athletes { get; set; } = new List<Athlete>();
    
    // Computed properties
    /// <summary>
    /// Current number of active athletes
    /// </summary>
    public int CurrentAthleteCount => Athletes?.Count(a => a.IsActive) ?? 0;
    
    /// <summary>
    /// Whether the trainer can accept more athletes
    /// </summary>
    public bool CanAcceptMoreAthletes => 
        IsAcceptingNewAthletes && 
        (!MaxAthletes.HasValue || CurrentAthleteCount < MaxAthletes.Value);
}
