using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Errors;

namespace BookManagement.Application.Books.Commands.SoftDeleteBooksBulk;

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
            {
                return Result.Failure(
                    DomainErrors.Book.NotFound(bookId));
            }

            book.SoftDelete();

            await bookRepository.UpdateAsync(book, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
