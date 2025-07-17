using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Commands.UpdateBook;

/// <summary>
/// Command to update book details.
/// </summary>
public sealed record UpdateBookCommand(
    Guid Id,
    string Title,
    int PublicationYear,
    string AuthorName) : ICommand;
