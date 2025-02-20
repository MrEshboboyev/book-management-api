namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to update a book.
/// </summary>
public sealed record UpdateBookRequest(
    Guid Id,
    string Title,
    int PublicationYear,
    string AuthorName);
