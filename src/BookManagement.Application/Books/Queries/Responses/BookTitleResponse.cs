using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Identity.Books;

namespace BookManagement.Application.Books.Queries.Responses;

/// <summary>
/// Represents the response model for a book title.
/// </summary>
public sealed record BookTitleResponse(
    BookId Id, 
    string Title)
{
    public static BookTitleResponse Create(Book book)
    {
        return new(
            book.Id,
            book.Title.Value);
    }
}
