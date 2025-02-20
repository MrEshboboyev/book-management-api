using BookManagement.Domain.Errors;
using BookManagement.Domain.Events.Books;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Domain.Entities.Books;

/// <summary>
/// Represents a book in the system.
/// </summary>
public sealed class Book : AggregateRoot, IAuditableEntity
{
    #region Constructors

    private Book(
        Guid id,
        Title title,
        PublicationYear publicationYear,
        Author author) : base(id)
    {
        Title = title;
        PublicationYear = publicationYear;
        Author = author;
        ViewsCount = 0;
    }

    private Book()
    {
    }

    #endregion

    #region Properties

    public Title Title { get; private set; }
    public PublicationYear PublicationYear { get; private set; }
    public Author Author { get; private set; }
    public int ViewsCount { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new book instance.
    /// </summary>
    public static Result<Book> Create(
        Guid id,
        Title title,
        PublicationYear publicationYear,
        Author author)
    {
        if (title is null || publicationYear is null || author is null)
        {
            return Result.Failure<Book>(
                DomainErrors.Book.InvalidData);
        }

        var book = new Book(id, title, publicationYear, author);

        book.RaiseDomainEvent(new BookCreatedDomainEvent(
            Guid.NewGuid(),
            book.Id,
            title.Value));

        return Result.Success(book);
    }

    #endregion

    #region Own Methods

    /// <summary>
    /// Increases the view count when the book details are accessed.
    /// </summary>
    public void AddView()
    {
        ViewsCount++;
        RaiseDomainEvent(new BookViewedDomainEvent(
            Guid.NewGuid(),
            Id,
            ViewsCount));
    }

    /// <summary>
    /// Updates the book details.
    /// </summary>
    public Result UpdateDetails(Title newTitle,
                              PublicationYear newYear,
                              Author newAuthor)
    {
        Title = newTitle;
        PublicationYear = newYear;
        Author = newAuthor;

        RaiseDomainEvent(new BookUpdatedDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    /// <summary>
    /// Soft deletes the book.
    /// </summary>
    public Result SoftDelete()
    {
        if (IsDeleted)
        {
            return Result.Failure(
                DomainErrors.Book.AlreadyDeleted);
        }

        IsDeleted = true;
        RaiseDomainEvent(new BookDeletedDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    /// <summary>
    /// Calculates the book's popularity score dynamically.
    /// </summary>
    public double CalculatePopularityScore()
    {
        var yearsSincePublished = DateTime.UtcNow.Year - PublicationYear.Value;
        return ViewsCount * 0.5 + yearsSincePublished * 2;
    }

    #endregion
}
