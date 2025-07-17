using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.ValueObjects.Users;
using BookManagement.Persistence.Users.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Persistence.Users.Configurations;

/// <summary> 
/// Configures the User entity for Entity Framework Core. 
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Map to the Users table
        builder.ToTable(UserTableNames.Users);

        builder.Property(x => x.Id);

        // Email Complex Property konfiguratsiyasi
        builder.ComplexProperty(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                        .HasColumnName("Email")
                        .IsRequired() // Email.Value majburiy, bu Email Value Objectining o'zini ham majburiy qiladi.
                        .HasMaxLength(Email.MaxLength);
        });

        // FirstName Complex Property konfiguratsiyasi
        builder.ComplexProperty(u => u.FirstName, firstNameBuilder =>
        {
            firstNameBuilder.Property(f => f.Value)
                            .HasColumnName("FirstName")
                            .IsRequired()
                            .HasMaxLength(FirstName.MaxLength);
        });

        // LastName Complex Property konfiguratsiyasi
        builder.ComplexProperty(u => u.LastName, lastNameBuilder =>
        {
            lastNameBuilder.Property(l => l.Value)
                            .HasColumnName("LastName")
                            .IsRequired()
                            .HasMaxLength(LastName.MaxLength);
        });

        //// Configure unique constraint on Email
        //builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => new { x.CreatedOnUtc, x.Id });
    }
}
