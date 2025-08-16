using Microsoft.EntityFrameworkCore;
using WorkoutProject.Domain.Entities;
using System.Linq.Expressions;

namespace WorkoutProject.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserClaim> UserClaims { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<Athlete> Athletes { get; set; }
    public DbSet<Trainer> Trainers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure global query filters for soft delete - but exclude junction tables
        ConfigureGlobalQueryFilters(modelBuilder);

        // Configure audit fields
        ConfigureAuditFields(modelBuilder);
    }

    private void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        // Apply soft delete filter to BaseEntity entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }

        // Apply matching filters to junction/child tables to prevent orphaned records
        ConfigureRelatedEntityFilters(modelBuilder);
    }

    private void ConfigureRelatedEntityFilters(ModelBuilder modelBuilder)
    {
        // UserClaim: Only show claims for non-deleted users
        modelBuilder.Entity<UserClaim>().HasQueryFilter(uc => !uc.User.IsDeleted);

        // UserLogin: Only show logins for non-deleted users
        modelBuilder.Entity<UserLogin>().HasQueryFilter(ul => !ul.User.IsDeleted);

        // UserRole: Only show user-role assignments for non-deleted users and non-deleted roles
        modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.User.IsDeleted && !ur.Role.IsDeleted);
    }

    private void ConfigureAuditFields(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.DateCreated))
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.DateUpdated))
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTimeOffset.UtcNow;
                    entry.Entity.DateUpdated = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.DateUpdated = DateTimeOffset.UtcNow;
                    break;
            }
        }
    }
}