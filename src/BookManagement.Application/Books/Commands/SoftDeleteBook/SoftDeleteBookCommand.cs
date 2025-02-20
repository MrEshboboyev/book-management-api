using BookManagement.Application.Abstractions.Messaging;

namespace BookManagement.Application.Books.Commands.SoftDeleteBook;

/// <summary>
/// Command to soft delete a book.
/// </summary>
public sealed record SoftDeleteBookCommand(Guid Id) : ICommand;
