namespace BookManagement.Application.Books.Queries.Common.Responses;

/// <summary>
/// Represents the response model for a book title.
/// </summary>
public sealed record BookTitleResponse(Guid Id, string Title);
