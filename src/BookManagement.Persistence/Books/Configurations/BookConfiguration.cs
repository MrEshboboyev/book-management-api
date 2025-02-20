using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookManagement.Domain.Entities.Books;
using BookManagement.Persistence.Books.Constants;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Persistence.Books.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(BookTableNames.Books);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value).Value)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.PublicationYear)
            .HasConversion(
                year => year.Value,
                value => PublicationYear.Create(value).Value)
            .IsRequired();

        builder.Property(b => b.Author)
            .HasConversion(
                author => author.Name,
                value => Author.Create(value).Value)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ViewsCount)
            .IsRequired();

        builder.Property(b => b.IsDeleted)
            .IsRequired();

        builder.Property(b => b.CreatedOnUtc)
            .IsRequired();

        builder.Property(b => b.ModifiedOnUtc);
    }
}
