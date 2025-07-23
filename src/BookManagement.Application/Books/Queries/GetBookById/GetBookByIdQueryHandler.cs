using BookManagement.Application.Books.Queries.Common.Factories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Books.Queries.Common.Responses;

namespace BookManagement.Application.Books.Queries.GetBookById;

internal sealed class GetBookByIdQueryHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork
) : IQueryHandler<GetBookByIdQuery, BookResponse>
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

        await bookRepository.UpdateAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var bookResponse = BookResponseFactory.Create(book);

        return Result.Success(bookResponse);
    }
}
