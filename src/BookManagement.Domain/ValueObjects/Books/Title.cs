using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Books;

/// <summary>
/// Represents the title of a book.
/// </summary>
public sealed class Title : ValueObject
{
    public const int MaxLength = 150;

    private Title(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Title> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<Title>(DomainErrors.Title.Empty);

        if (title.Length > MaxLength)
            return Result.Failure<Title>(DomainErrors.Title.TooLong);

        return Result.Success(new Title(title));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
