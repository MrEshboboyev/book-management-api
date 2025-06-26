using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Identity.Books;

namespace BookManagement.Application.Books.Queries.Responses;

/// <summary>
/// Represents the response model for a book.
/// </summary>
public sealed record BookResponse(
    string Id,
    string Title,
    int PublicationYear,
    string AuthorName,
    int ViewsCount,
    double PopularityScore)
{
    public static BookResponse Create(Book book)
    {
        return new BookResponse(
            book.Id.ToString(),
            book.Title.Value,
            book.PublicationYear.Value,
            book.Author.Name,
            book.ViewsCount,
            book.CalculatePopularityScore());
    }
}
