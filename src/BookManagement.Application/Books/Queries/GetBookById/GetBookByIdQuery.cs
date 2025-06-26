using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Books.Queries.Responses;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(BookId Id) : IQuery<BookResponse>;

internal sealed class GetBookByIdQueryHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(
        GetBookByIdQuery request,
        CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
            return Result.Failure<BookResponse>(DomainErrors.Book.NotFound(request.Id));

        book.AddView(); // Increase view count on retrieval

        await bookRepository.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var bookResponse = BookResponse.Create(book);

        return Result.Success(bookResponse);
    }
}
