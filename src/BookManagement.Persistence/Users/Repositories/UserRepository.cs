using BookManagement.Domain.Entities;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Set<User>().ToListAsync(cancellationToken);

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(member => member.Id == id, cancellationToken);

    public async Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(member => member.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        !await _dbContext
            .Set<User>()
            .AnyAsync(member => member.Email == email, cancellationToken);

    public void Add(User member) =>
        _dbContext.Set<User>().Add(member);

    public void Update(User member) =>
        _dbContext.Set<User>().Update(member);
}