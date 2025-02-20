namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to add a new book.
/// </summary>
public sealed record AddBookRequest(
    string Title,
    int PublicationYear,
    string AuthorName);
