using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Commands.AddBooksBulk;

/// <summary>
/// Command to add multiple books in bulk.
/// </summary>
public sealed record AddBooksBulkCommand(
    List<(
        string Title, 
        int PublicationYear, 
        string AuthorName)> Books) : ICommand<List<Guid>>;
