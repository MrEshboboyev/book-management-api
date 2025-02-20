using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Books;

/// <summary>
/// Represents an author of a book.
/// </summary>
public sealed class Author : ValueObject
{
    private Author(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static Result<Author> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Author>(DomainErrors.Author.Empty);

        return Result.Success(new Author(name));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}
