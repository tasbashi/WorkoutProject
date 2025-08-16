using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkoutProject.Domain.Entities;
using WorkoutProject.Infrastructure.Data;

namespace WorkoutProject.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("DatabaseSeeder");

        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed roles
            await SeedRolesAsync(context, logger);

            // Seed admin user
            await SeedAdminUserAsync(context, passwordHasher, logger);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context, ILogger logger)
    {
        var roles = new[]
        {
            new Role { Name = "Admin", NormalizedName = "ADMIN", Description = "System Administrator", IsSystemRole = true },
            new Role { Name = "Trainer", NormalizedName = "TRAINER", Description = "Fitness Trainer", IsSystemRole = false },
            new Role { Name = "Athlete", NormalizedName = "ATHLETE", Description = "Athlete/Client", IsSystemRole = false }
        };

        foreach (var role in roles)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == role.Name))
            {
                role.Id = Guid.NewGuid();
                role.CreatedBy = Guid.Empty;
                role.UpdatedBy = Guid.Empty;
                context.Roles.Add(role);
                logger.LogInformation("Seeded role: {RoleName}", role.Name);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ILogger logger)
    {
        const string adminUsername = "testadmin";
        const string adminEmail = "admin@workoutproject.com";
        const string adminPassword = "Admin123!";

        if (!await context.Users.AnyAsync(u => u.Username == adminUsername))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = adminUsername,
                Email = adminEmail,
                FirstName = "Test",
                LastName = "Admin",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedBy = Guid.Empty,
                UpdatedBy = Guid.Empty
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, adminPassword);

            context.Users.Add(adminUser);

            // Assign Admin role
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id,
                AssignedBy = adminUser.Id
            };

            context.UserRoles.Add(userRole);

            logger.LogInformation("Seeded admin user: {Username} with password: {Password}", adminUsername, adminPassword);
        }
    }
}