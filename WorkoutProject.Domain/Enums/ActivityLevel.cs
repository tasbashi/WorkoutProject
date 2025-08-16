namespace WorkoutProject.Domain.Enums;

/// <summary>
/// Represents different activity levels for athletes
/// </summary>
public enum ActivityLevel
{
    /// <summary>
    /// Little to no exercise
    /// </summary>
    Sedentary = 0,
    
    /// <summary>
    /// Light exercise/sports 1-3 days/week
    /// </summary>
    Light = 1,
    
    /// <summary>
    /// Moderate exercise/sports 3-5 days/week
    /// </summary>
    Moderate = 2,
    
    /// <summary>
    /// Hard exercise/sports 6-7 days a week
    /// </summary>
    Active = 3,
    
    /// <summary>
    /// Very hard exercise/sports and physical job or 2x training
    /// </summary>
    VeryActive = 4
}
