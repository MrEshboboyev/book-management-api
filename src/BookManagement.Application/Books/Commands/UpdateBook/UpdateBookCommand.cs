using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Application.Books.Commands.UpdateBook;

/// <summary>
/// Command to update book details.
/// </summary>
public sealed record UpdateBookCommand(
    BookId Id,
    string Title,
    int PublicationYear,
    string AuthorName) : ICommand;

internal sealed class UpdateBookCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateBookCommand>
{
    public async Task<Result> Handle(
        UpdateBookCommand request,
        CancellationToken cancellationToken)
    {
        var (bookId, title, publicationYear, authorName) = request;

        var book = await bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            return Result.Failure(DomainErrors.Book.NotFound(bookId));
        }

        var titleResult = Title.Create(title);
        if (titleResult.IsFailure)
        {
            return Result.Failure<Guid>(
                titleResult.Error);
        }

        var bookExists = await bookRepository.ExistsByTitleAsync(titleResult.Value, cancellationToken);
        if (bookExists)
        {
            return Result.Failure(
                DomainErrors.Book.AlreadyExists(title));
        }

        #region Prepare value objects with create methods

        var authorResult = Author.Create(authorName);
        if (authorResult.IsFailure)
        {
            return Result.Failure<Guid>(
                authorResult.Error);
        }

        var publicationYearResult = PublicationYear.Create(publicationYear);
        if (publicationYearResult.IsFailure)
        {
            return Result.Failure<Guid>(
                publicationYearResult.Error);
        }

        #endregion

        var updateBookResult = book.UpdateDetails(
            titleResult.Value,
            publicationYearResult.Value,
            authorResult.Value);
        if (updateBookResult.IsFailure)
        {
            return Result.Failure<Guid>(
                updateBookResult.Error);
        }

        await bookRepository.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
