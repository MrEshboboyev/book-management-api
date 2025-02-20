namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to soft delete a book.
/// </summary>
public sealed record SoftDeleteBookRequest(Guid Id);
