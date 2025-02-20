namespace BookManagement.Application.Books.Queries.Common;

/// <summary>
/// Represents the response model for a book.
/// </summary>
public sealed record BookResponse(
    Guid Id,
    string Title,
    int PublicationYear,
    string AuthorName,
    int ViewsCount,
    double PopularityScore);
