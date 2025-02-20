using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.Repositories.Books;

public interface IBookRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetBooksAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Book> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    // Other method signatures
}
