namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to add multiple books in bulk.
/// </summary>
public sealed record AddBooksBulkRequest(
    List<(
        string Title,
        int PublicationYear,
        string AuthorName)> Books);