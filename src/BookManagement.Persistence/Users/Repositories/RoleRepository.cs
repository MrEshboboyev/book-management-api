using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence.Users.Repositories;

public sealed class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    public async Task<Role> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Role>()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<Role> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
        => await dbContext
                .Set<Role>()
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext
                .Set<Role>()
                .ToListAsync(cancellationToken);


    public void Add(Role role)
        => dbContext
                .Set<Role>()
                .Add(role);

    public void Update(Role role)
        => dbContext
                .Set<Role>()
                .Update(role);
}