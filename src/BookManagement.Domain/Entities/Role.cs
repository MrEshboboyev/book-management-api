using BookManagement.Domain.Primitives;

namespace BookManagement.Domain.Entities;

/// <summary> 
/// Represents a user role in the system. 
/// </summary>
public sealed class Role(
    int id,
    string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Registered = new(1, "Registered");

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<User> Users { get; set; }
}