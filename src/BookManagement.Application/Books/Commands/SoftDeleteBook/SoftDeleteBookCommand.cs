using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Books.Commands.SoftDeleteBook;

/// <summary>
/// Command to soft delete a book.
/// </summary>
public sealed record SoftDeleteBookCommand(
    BookId Id) : ICommand;

internal sealed class SoftDeleteBookCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SoftDeleteBookCommand>
{
    public async Task<Result> Handle(
        SoftDeleteBookCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Id;

        var book = await bookRepository.GetByIdAsync(id, cancellationToken);
        if (book is null)
            return Result.Failure(DomainErrors.Book.NotFound(id));

        var softDeleteResult = book.SoftDelete();
        if (softDeleteResult.IsFailure)
            return Result.Failure(softDeleteResult.Error);

        await bookRepository.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
