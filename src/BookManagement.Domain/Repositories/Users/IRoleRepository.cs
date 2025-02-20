using BookManagement.Domain.Entities.Users;

namespace BookManagement.Domain.Repositories.Users;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}