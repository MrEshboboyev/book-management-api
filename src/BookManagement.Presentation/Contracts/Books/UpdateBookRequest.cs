using BookManagement.Domain.Identity.Books;

namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to update a book.
/// </summary>
public sealed record UpdateBookRequest(
    BookId Id,
    string Title,
    int PublicationYear,
    string AuthorName);
