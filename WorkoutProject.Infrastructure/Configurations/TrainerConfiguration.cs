using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for Trainer entity
/// </summary>
public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        // Table name
        builder.ToTable("Trainers");

        // Primary key (inherited from BaseEntity)
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(t => t.UserId)
            .IsRequired();

        builder.Property(t => t.Specializations)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("JSON array of specializations");

        builder.Property(t => t.Certifications)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("JSON array of certification objects");

        builder.Property(t => t.Experience)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Professional experience description");

        builder.Property(t => t.Bio)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Professional bio/description");

        builder.Property(t => t.HourlyRate)
            .HasPrecision(8, 2); // Up to 999,999.99

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(t => t.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.Rating)
            .HasPrecision(3, 2); // 0.00 to 5.00

        builder.Property(t => t.TotalReviews)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(t => t.MaxAthletes)
            .HasDefaultValue(50);

        builder.Property(t => t.IsAcceptingNewAthletes)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.TimeZone)
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(t => t.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Trainers_UserId");

        builder.HasIndex(t => t.IsVerified)
            .HasDatabaseName("IX_Trainers_IsVerified");

        builder.HasIndex(t => t.Rating)
            .HasDatabaseName("IX_Trainers_Rating");

        builder.HasIndex(t => t.IsAcceptingNewAthletes)
            .HasDatabaseName("IX_Trainers_IsAcceptingNewAthletes");

        builder.HasIndex(t => t.VerificationDate)
            .HasDatabaseName("IX_Trainers_VerificationDate");

        // Relationships
        builder.HasOne(t => t.User)
            .WithOne(u => u.Trainer)
            .HasForeignKey<Trainer>(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Trainers_Users");

        builder.HasMany(t => t.Athletes)
            .WithOne(a => a.Trainer)
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Athletes_Trainers");

        // Check constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Trainers_HourlyRate_Positive", "[HourlyRate] >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Trainers_Rating_Range", "[Rating] BETWEEN 0.00 AND 5.00"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Trainers_TotalReviews_NonNegative", "[TotalReviews] >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Trainers_MaxAthletes_Positive", "[MaxAthletes] > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Trainers_Currency_Length", "LEN([Currency]) = 3"));

        // Computed properties (read-only)
        builder.Ignore(t => t.CurrentAthleteCount);
        builder.Ignore(t => t.CanAcceptMoreAthletes);
    }
}
