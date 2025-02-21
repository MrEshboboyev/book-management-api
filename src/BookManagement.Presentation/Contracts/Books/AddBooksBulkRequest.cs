namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to add multiple books in bulk.
/// </summary>
public sealed record AddBooksBulkRequest(
    List<BookDetails> Books);

/// <summary>
/// Represents the details of a book.
/// </summary>
public sealed record BookDetails(
    string Title,
    int PublicationYear,
    string AuthorName);
