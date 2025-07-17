using BookManagement.Application.Books.Queries.Common.Responses;
using BookManagement.Domain.Entities.Books;

namespace BookManagement.Application.Books.Queries.Common.Factories;

/// <summary>
/// Factory to create BookResponse instances.
/// </summary>
public static class BookResponseFactory
{
    public static BookResponse Create(Book book)
    {
        return new BookResponse(
            book.Id,
            book.Title.Value,
            book.PublicationYear.Value,
            book.Author.Name,
            book.ViewsCount,
            book.CalculatePopularityScore());
    }
}
