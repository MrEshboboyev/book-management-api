using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;

/// <summary>
/// Command to soft delete multiple books.
/// </summary>
public sealed record SoftDeleteBooksBulkCommand(
    List<BookId> BookIds) : ICommand;

internal sealed class SoftDeleteBooksBulkCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SoftDeleteBooksBulkCommand>
{
    public async Task<Result> Handle(
        SoftDeleteBooksBulkCommand request,
        CancellationToken cancellationToken)
    {
        var bookIds = request.BookIds;

        foreach (var bookId in bookIds)
        {
            var book = await bookRepository.GetByIdAsync(bookId, cancellationToken);
            if (book is null)
                return Result.Failure(DomainErrors.Book.NotFound(bookId));

            book.SoftDelete();

            await bookRepository.UpdateAsync(book, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
