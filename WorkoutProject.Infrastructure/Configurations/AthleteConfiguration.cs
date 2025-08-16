using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for Athlete entity
/// </summary>
public class AthleteConfiguration : IEntityTypeConfiguration<Athlete>
{
    public void Configure(EntityTypeBuilder<Athlete> builder)
    {
        // Table name
        builder.ToTable("Athletes");

        // Primary key (inherited from BaseEntity)
        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.Height)
            .HasPrecision(5, 2); // Up to 999.99 cm

        builder.Property(a => a.Weight)
            .HasPrecision(5, 2); // Up to 999.99 kg

        builder.Property(a => a.ActivityLevel)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Goals)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("JSON array of fitness goals");

        builder.Property(a => a.MedicalHistory)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Medical history information");

        builder.Property(a => a.Allergies)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Known allergies");

        builder.Property(a => a.EmergencyContactName)
            .HasMaxLength(100);

        builder.Property(a => a.EmergencyContactPhone)
            .HasMaxLength(20);

        builder.Property(a => a.EmergencyContactRelationship)
            .HasMaxLength(50);

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(a => a.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Athletes_UserId");

        builder.HasIndex(a => a.TrainerId)
            .HasDatabaseName("IX_Athletes_TrainerId");

        builder.HasIndex(a => a.IsActive)
            .HasDatabaseName("IX_Athletes_IsActive");

        builder.HasIndex(a => a.JoinedTrainerAt)
            .HasDatabaseName("IX_Athletes_JoinedTrainerAt");

        // Relationships
        builder.HasOne(a => a.User)
            .WithOne(u => u.Athlete)
            .HasForeignKey<Athlete>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Athletes_Users");

        builder.HasOne(a => a.Trainer)
            .WithMany(t => t.Athletes)
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Athletes_Trainers");

        // Check constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Athletes_Height_Positive", "[Height] > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Athletes_Weight_Positive", "[Weight] > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Athletes_Height_Realistic", "[Height] BETWEEN 50 AND 300")); // 50cm to 300cm
        builder.ToTable(t => t.HasCheckConstraint("CK_Athletes_Weight_Realistic", "[Weight] BETWEEN 10 AND 500")); // 10kg to 500kg

        // Computed property BMI (read-only)
        builder.Ignore(a => a.BMI);
    }
}
