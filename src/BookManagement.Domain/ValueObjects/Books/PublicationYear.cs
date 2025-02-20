using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Books;

/// <summary>
/// Represents the publication year of a book.
/// </summary>
public sealed class PublicationYear : ValueObject
{
    private PublicationYear(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<PublicationYear> Create(int year)
    {
        var currentYear = DateTime.UtcNow.Year;

        if (year < 1450 || year > currentYear)
            return Result.Failure<PublicationYear>(DomainErrors.PublicationYear.Invalid);

        return Result.Success(new PublicationYear(year));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
