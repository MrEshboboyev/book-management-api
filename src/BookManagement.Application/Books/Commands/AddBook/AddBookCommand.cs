using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Commands.AddBook;

/// <summary>
/// Command to add a new book.
/// </summary>
/// <param name="Title">The title of the book.</param>
/// <param name="PublicationYear">The publication year of the book.</param>
/// <param name="AuthorName">The author's name.</param>
public sealed record AddBookCommand(
    string Title,
    int PublicationYear,
    string AuthorName) : ICommand<Guid>;
