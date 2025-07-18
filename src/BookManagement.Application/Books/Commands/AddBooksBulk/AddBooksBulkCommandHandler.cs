﻿using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects.Books;
using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Books.Commands.AddBooksBulk;

/// <summary>
/// Handles the command to add multiple books in bulk.
/// </summary>
internal sealed class AddBooksBulkCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddBooksBulkCommand, List<Guid>>
{
    public async Task<Result<List<Guid>>> Handle(
        AddBooksBulkCommand request,
        CancellationToken cancellationToken)
    {
        var bookIds = new List<Guid>();

        foreach (var (title, publicationYear, authorName) in request.Books)
        {
            var titleResult = Title.Create(title);
            if (titleResult.IsFailure)
            {
                return Result.Failure<List<Guid>>(
                    titleResult.Error);
            }

            // Check if book already exists
            if (await bookRepository.ExistsByTitleAsync(titleResult.Value, cancellationToken))
            {
                return Result.Failure<List<Guid>>(
                    DomainErrors.Book.AlreadyExists(title));
            }

            #region Prepare value objects with create methods

            var authorResult = Author.Create(authorName);
            if (authorResult.IsFailure)
            {
                return Result.Failure<List<Guid>>(
                    authorResult.Error);
            }

            var publicationYearResult = PublicationYear.Create(publicationYear);
            if (publicationYearResult.IsFailure)
            {
                return Result.Failure<List<Guid>>(
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