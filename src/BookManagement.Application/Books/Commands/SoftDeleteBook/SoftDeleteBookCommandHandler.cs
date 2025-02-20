using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories;

namespace BookManagement.Application.Books.Commands.SoftDeleteBook;

internal sealed class SoftDeleteBookCommandHandler(
    IBookRepository bookRepository, 
    IUnitOfWork unitOfWork): ICommandHandler<SoftDeleteBookCommand>
{
    public async Task<Result> Handle(
        SoftDeleteBookCommand request, 
        CancellationToken cancellationToken)
    {
        var id = request.Id;

        var book = await bookRepository.GetByIdAsync(id, cancellationToken);
        if (book is null)
        {
            return Result.Failure(
                DomainErrors.Book.NotFound(id));
        }

        var softDeleteResult = book.SoftDelete();
        if (softDeleteResult.IsFailure)
        {
            return Result.Failure(softDeleteResult.Error);
        }

        await bookRepository.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
