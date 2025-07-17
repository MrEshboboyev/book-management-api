using BookManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Application.Common.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; } // Expose DbSet for User entity
}
