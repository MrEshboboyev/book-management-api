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
        // Email as simple converted property
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(Email.MaxLength);

        // FirstName as simple converted property
        builder.Property(u => u.FirstName)
            .HasConversion(
                fn => fn.Value,
                value => FirstName.Create(value).Value)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(FirstName.MaxLength);

        // LastName as simple converted property
        builder.Property(u => u.LastName)
            .HasConversion(
                ln => ln.Value,
                value => LastName.Create(value).Value)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(LastName.MaxLength);

        // optional: unique index on email
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(x => new { x.CreatedOnUtc, x.Id });
    }
}
