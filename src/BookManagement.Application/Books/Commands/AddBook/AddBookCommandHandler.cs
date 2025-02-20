using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Application.Books.Commands.AddBook;

/// <summary>
/// Handles the command to add a book.
/// </summary>
internal sealed class AddBookCommandHandler(
    IBookRepository bookRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<AddBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        AddBookCommand request, 
        CancellationToken cancellationToken)
    {
        var (title, publicationYear, authorName) = request;

        // Check if book already exists
        if (await bookRepository.ExistsByTitleAsync(title, cancellationToken))
        {
            return Result.Failure<Guid>(
                DomainErrors.Book.AlreadyExists(title));
        }

        #region Prepare value objects with create methods

        var titleResult = Title.Create(title);
        if (titleResult.IsFailure)
        {
            return Result.Failure<Guid>(
                titleResult.Error);
        }

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

        #region Create new book
        
        var bookResult = Book.Create(Guid.NewGuid(),
                               titleResult.Value,
                               publicationYearResult.Value,
                               authorResult.Value);

        #endregion

        #region Add and update database

        await bookRepository.AddAsync(bookResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success(bookResult.Value.Id);
    }
}
