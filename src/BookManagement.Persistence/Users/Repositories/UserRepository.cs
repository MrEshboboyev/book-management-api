using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.ValueObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence.Users.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<IEnumerable<User>> SearchAsync(
        string email,
        string name,
        int? roleId,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<User>().AsQueryable();

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(user => user.Email.Value == email);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(user => user.FirstName.Value == name
                                        || user.LastName.Value == name);
        }

        if (roleId.HasValue)
        {
            query = query
                .Include(user => user.Roles)
                .Where(user => user.Roles.Any(role => role.Id == roleId));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<User>()
            .ToListAsync(cancellationToken);

    public async Task<User> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        await dbContext
            .Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        !await dbContext
            .Set<User>()
            .AnyAsync(user => user.Email == email, cancellationToken);

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<User>()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await dbContext.Set<User>().AddAsync(user, cancellationToken);

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        => dbContext.Set<User>().Update(user);

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
        => dbContext.Set<User>().Remove(user);
}