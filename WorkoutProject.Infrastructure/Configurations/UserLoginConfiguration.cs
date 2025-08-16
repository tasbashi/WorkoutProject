using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutProject.Domain.Entities;

namespace WorkoutProject.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for UserLogin entity
/// </summary>
public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        // Table name
        builder.ToTable("UserLogins");

        // Composite primary key
        builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

        // Properties
        builder.Property(ul => ul.LoginProvider)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(ul => ul.ProviderKey)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(ul => ul.ProviderDisplayName)
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(ul => ul.UserId)
            .HasDatabaseName("IX_UserLogins_UserId");

        // Relationships
        builder.HasOne(ul => ul.User)
            .WithMany(u => u.UserLogins)
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserLogins_Users");

        // Check constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_UserLogins_LoginProvider_Length", "LEN([LoginProvider]) > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_UserLogins_ProviderKey_Length", "LEN([ProviderKey]) > 0"));
    }
}
