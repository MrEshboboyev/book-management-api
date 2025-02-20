namespace BookManagement.Domain.Entities;

/// <summary> 
/// Represents the relationship between roles and permissions. 
/// </summary>
public class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}