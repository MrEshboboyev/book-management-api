using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Books.Queries.Common;
using BookManagement.Application.Books.Queries.Common.Factories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Books.Queries.GetBookById;

internal sealed class GetBookByIdQueryHandler(
    IBookRepository bookRepository) : IQueryHandler<GetBookByIdQuery, BookResponse>
{
    public async Task<Result<BookResponse>> Handle(
        GetBookByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
        {
            return Result.Failure<BookResponse>(DomainErrors.Book.NotFound(request.Id));
        }

        book.AddView(); // Increase view count on retrieval

        var bookResponse = BookResponseFactory.Create(book);

        return Result.Success(bookResponse);
    }
}
