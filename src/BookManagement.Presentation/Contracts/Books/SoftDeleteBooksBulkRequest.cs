namespace BookManagement.Presentation.Contracts.Books;

/// <summary>
/// Request to soft delete multiple books in bulk.
/// </summary>
public sealed record SoftDeleteBooksBulkRequest(List<Guid> BookIds);
