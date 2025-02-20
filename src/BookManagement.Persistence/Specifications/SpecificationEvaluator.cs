using BookManagement.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence.Specifications;

/// <summary> 
/// Evaluates and applies a specification to a given IQueryable to filter, include related entities, and order results. 
/// </summary>
public static class SpecificationEvaluator
{
    /// <summary> 
    /// Applies the specified criteria, includes, and ordering to the input queryable. 
    /// </summary> 
    /// <typeparam name="TEntity">The type of the entity.</typeparam> 
    /// <param name="inputQueryable">The initial queryable.</param> 
    /// <param name="specification">The specification to apply.</param> 
    /// <returns>The queryable with the specification applied.</returns>
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        var queryable = inputQueryable;

        // Apply criteria if specified
        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        // Apply to include expressions
        _ = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpression) =>
            current.Include(includeExpression));

        // Apply ordering if specified
        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(
                specification.OrderByDescendingExpression);
        }

        // Apply split query if specified
        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}
