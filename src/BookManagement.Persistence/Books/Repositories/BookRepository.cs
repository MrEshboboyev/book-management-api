using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Repositories.Books;
using BookManagement.Domain.ValueObjects.Books;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence.Books.Repositories;

public class BookRepository(ApplicationDbContext context) : IBookRepository
{
    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await context.Set<Book>().AddAsync(book, cancellationToken);
    }

    public async Task<bool> ExistsByTitleAsync(
        Title title, 
        CancellationToken cancellationToken = default)
    {
        return await context.Set<Book>().AnyAsync(b => b.Title == title, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await context.Set<Book>()
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.ViewsCount)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Book> GetByIdAsync(BookId id, CancellationToken cancellationToken = default)
    {
        return await context.Set<Book>()
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        await Task.Delay(10);
        context.Set<Book>().Update(book);
    }
}
