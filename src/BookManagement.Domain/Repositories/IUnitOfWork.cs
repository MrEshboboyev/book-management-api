using System.Data;

namespace BookManagement.Domain.Repositories;

/// <summary> 
/// Defines the contract for a unit of work pattern. 
/// </summary>
public interface IUnitOfWork
{
    /// <summary> 
    /// Saves changes to the database asynchronously. 
    /// </summary> 
    /// <param name="cancellationToken">Cancellation token.</param> 
    /// <returns>Task representing the save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary> 
    /// Begins a new database transaction. 
    /// </summary> 
    /// <returns>IDbTransaction representing the transaction.</returns>
    IDbTransaction BeginTransaction();
}

