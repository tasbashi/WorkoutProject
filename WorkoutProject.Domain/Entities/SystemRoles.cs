namespace WorkoutProject.Domain.Entities;

/// <summary>
/// Contains constants for system roles
/// </summary>
public static class SystemRoles
{
    public const string Admin = "Admin";
    public const string Trainer = "Trainer";
    public const string Athlete = "Athlete";
    public const string Nutritionist = "Nutritionist";

    public const string AdminNormalized = "ADMIN";
    public const string TrainerNormalized = "TRAINER";
    public const string AthleteNormalized = "ATHLETE";
    public const string NutritionistNormalized = "NUTRITIONIST";

    /// <summary>
    /// Gets all system role names
    /// </summary>
    public static string[] GetAllRoles()
    {
        return new[] { Admin, Trainer, Athlete, Nutritionist };
    }

    /// <summary>
    /// Gets all normalized system role names
    /// </summary>
    public static string[] GetAllNormalizedRoles()
    {
        return new[] { AdminNormalized, TrainerNormalized, AthleteNormalized, NutritionistNormalized };
    }
}