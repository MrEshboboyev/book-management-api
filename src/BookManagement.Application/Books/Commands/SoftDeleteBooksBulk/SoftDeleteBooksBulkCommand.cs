using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;

/// <summary>
/// Command to soft delete multiple books.
/// </summary>
public sealed record SoftDeleteBooksBulkCommand(List<Guid> BookIds) : ICommand;
