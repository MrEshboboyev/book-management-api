namespace BookManagement.Domain.Entities;

/// <summary> 
/// Represents a user role in the system. 
/// </summary>
public class Permission
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
