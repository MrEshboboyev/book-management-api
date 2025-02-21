using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Books.Queries.Common;
using BookManagement.Application.Common.Models;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork): IQueryHandler<GetBooksQuery, PaginatedList<BookTitleResponse>>
{
    public async Task<Result<PaginatedList<BookTitleResponse>>> Handle(
        GetBooksQuery request, 
        CancellationToken cancellationToken)
    {
        var (pageNumber, pageSize) = request;

        var books = await bookRepository.GetBooksAsync(
            pageNumber, 
            pageSize, 
            cancellationToken);

        foreach (var book in books)
        {
            book.AddView(); // Increase view count on retrieval

            await bookRepository.UpdateAsync(book, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var bookTitles = books.Select(book => 
            new BookTitleResponse(book.Id, book.Title.Value)).ToList();

        return Result.Success(
            new PaginatedList<BookTitleResponse>(bookTitles, pageNumber, pageSize, books.Count()));
    }
}
