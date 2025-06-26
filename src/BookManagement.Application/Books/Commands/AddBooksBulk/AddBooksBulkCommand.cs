using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Application.Books.Commands.AddBooksBulk;

/// <summary>
/// Command to add multiple books in bulk.
/// </summary>
public sealed record AddBooksBulkCommand(
    List<(
        string Title, 
        int PublicationYear, 
        string AuthorName)> Books) : ICommand<List<BookId>>;

internal sealed class AddBooksBulkCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddBooksBulkCommand, List<BookId>>
{
    public async Task<Result<List<BookId>>> Handle(
        AddBooksBulkCommand request,
        CancellationToken cancellationToken)
    {
        var bookIds = new List<BookId>();

        foreach (var (title, publicationYear, authorName) in request.Books)
        {
            var titleResult = Title.Create(title);
            if (titleResult.IsFailure)
            {
                return Result.Failure<List<BookId>>(
                    titleResult.Error);
            }

            // Check if book already exists
            if (await bookRepository.ExistsByTitleAsync(titleResult.Value, cancellationToken))
            {
                return Result.Failure<List<BookId>>(
                    DomainErrors.Book.AlreadyExists(title));
            }

            #region Prepare value objects with create methods

            var authorResult = Author.Create(authorName);
            if (authorResult.IsFailure)
            {
                return Result.Failure<List<BookId>>(
                    authorResult.Error);
            }

            var publicationYearResult = PublicationYear.Create(publicationYear);
            if (publicationYearResult.IsFailure)
            {
                return Result.Failure<List<BookId>>(
                    publicationYearResult.Error);
            }

            #endregion

            #region Create new book

            var bookResult = Book.Create(
                titleResult.Value,
                publicationYearResult.Value,
                authorResult.Value);

            #endregion

            #region Add and update database

            await bookRepository.AddAsync(bookResult.Value, cancellationToken);
            bookIds.Add(bookResult.Value.Id);

            #endregion
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(bookIds);
    }
}
