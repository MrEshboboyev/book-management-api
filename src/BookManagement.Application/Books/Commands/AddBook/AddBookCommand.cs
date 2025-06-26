using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Books;

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
    string AuthorName) : ICommand<BookId>;

internal sealed class AddBookCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddBookCommand, BookId>
{
    public async Task<Result<BookId>> Handle(
        AddBookCommand request,
        CancellationToken cancellationToken)
    {
        var (title, publicationYear, authorName) = request;

        var titleResult = Title.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure<BookId>(titleResult.Error);

        // Check if book already exists
        if (await bookRepository.ExistsByTitleAsync(titleResult.Value, cancellationToken))
            return Result.Failure<BookId>(DomainErrors.Book.AlreadyExists(title));

        #region Prepare value objects with create methods

        var authorResult = Author.Create(authorName);
        if (authorResult.IsFailure)
            return Result.Failure<BookId>(authorResult.Error);

        var publicationYearResult = PublicationYear.Create(publicationYear);
        if (publicationYearResult.IsFailure)
            return Result.Failure<BookId>(publicationYearResult.Error);

        #endregion

        #region Create new book

        var bookResult = Book.Create(
            titleResult.Value,
            publicationYearResult.Value,
            authorResult.Value);
        if (bookResult.IsFailure)
            return Result.Failure<BookId>(bookResult.Error);

        #endregion

        #region Add and update database

        await bookRepository.AddAsync(bookResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success(bookResult.Value.Id);
    }
}
