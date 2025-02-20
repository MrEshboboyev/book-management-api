using BookManagement.Domain.Entities.Books;

namespace BookManagement.Application.Books.Queries.Common.Factories;

/// <summary>
/// Factory to create BookResponse instances.
/// </summary>
public static class BookResponseFactory
{
    public static BookResponse Create(Book book)
    {
        var popularityScore = CalculatePopularityScore(book.ViewsCount, book.PublicationYear.Value);
        return new BookResponse(
            book.Id,
            book.Title.Value,
            book.PublicationYear.Value,
            book.Author.Name,
            book.ViewsCount,
            popularityScore);
    }

    private static double CalculatePopularityScore(int viewsCount, int publicationYear)
    {
        var yearsSincePublished = DateTime.UtcNow.Year - publicationYear;
        return viewsCount * 0.5 + yearsSincePublished * 2;
    }
}
