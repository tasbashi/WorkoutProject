using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for RefreshToken entity
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Table name
        builder.ToTable("RefreshTokens");

        // Primary key
        builder.HasKey(rt => rt.Id);

        // Properties
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(rt => rt.JwtId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(rt => rt.RevokedByIp)
            .HasMaxLength(45);

        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens_Token");

        builder.HasIndex(rt => rt.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserId");

        builder.HasIndex(rt => rt.ExpiryDate)
            .HasDatabaseName("IX_RefreshTokens_ExpiryDate");

        builder.HasIndex(rt => rt.JwtId)
            .HasDatabaseName("IX_RefreshTokens_JwtId");

        // Relationships
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RefreshTokens_Users");

        // Check constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_RefreshTokens_Token_Length", "LEN([Token]) > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_RefreshTokens_JwtId_Length", "LEN([JwtId]) > 0"));
    }
}
