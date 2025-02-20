namespace BookManagement.Domain.Primitives;

/// <summary> 
/// Defines properties for auditable entities. 
/// </summary>
public interface IAuditableEntity
{
    /// <summary> 
    /// Gets or sets the creation timestamp in UTC. 
    /// </summary> 
    DateTime CreatedOnUtc { get; set; } 
    
    /// <summary> 
    /// Gets or sets the modification timestamp in UTC. 
    /// </summary> 
    DateTime? ModifiedOnUtc { get; set; }
}
