using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for UserClaim entity
/// </summary>
public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        // Table name
        builder.ToTable("UserClaims");

        // Primary key
        builder.HasKey(uc => uc.Id);

        // Properties
        builder.Property(uc => uc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(uc => uc.ClaimType)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(uc => uc.ClaimValue)
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(uc => uc.UserId)
            .HasDatabaseName("IX_UserClaims_UserId");

        builder.HasIndex(uc => uc.ClaimType)
            .HasDatabaseName("IX_UserClaims_ClaimType");

        // Relationships
        builder.HasOne(uc => uc.User)
            .WithMany(u => u.UserClaims)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserClaims_Users");

        // Check constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_UserClaims_ClaimType_Length", "LEN([ClaimType]) > 0"));
    }
}
