using BookManagement.Domain.Primitives;
using System.Linq.Expressions;

namespace BookManagement.Persistence.Specifications;

/// <summary> 
/// Represents a specification pattern used to encapsulate the query logic in a reusable, combinable, and testable manner. 
/// </summary> 
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class Specification<TEntity>
    where TEntity : Entity
{
    /// <summary> 
    /// Initializes a new instance of the <see cref="Specification{TEntity}"/> class with the specified criteria. 
    /// </summary> 
    /// <param name="criteria">The criteria to filter the entities.</param>
    protected Specification(Expression<Func<TEntity, bool>> criteria) =>
        Criteria = criteria;

    /// <summary> 
    /// Gets or sets a value indicating whether to use split query. 
    /// </summary>
    public bool IsSplitQuery { get; protected set; }

    /// <summary> 
    /// Gets the criteria expression to filter the entities. 
    /// </summary>
    public Expression<Func<TEntity, bool>> Criteria { get; }

    /// <summary> 
    /// Gets the list of include expressions for related entities. 
    /// </summary>
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

    /// <summary> 
    /// Gets the order by expression for sorting the entities. 
    /// </summary>
    public Expression<Func<TEntity, object>> OrderByExpression { get; private set; }

    /// <summary> 
    /// Gets the order by descending expression for sorting the entities. 
    /// </summary>
    public Expression<Func<TEntity, object>> OrderByDescendingExpression { get; private set; }

    /// <summary> 
    /// Adds an include expression for related entities. 
    /// </summary> 
    /// <param name="includeExpression">The include expression to add.</param>
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(includeExpression);

    /// <summary> 
    /// Adds an order by expression for sorting the entities. 
    /// </summary> 
    /// <param name="orderByExpression">The order by expression to add.</param>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpression = orderByExpression;

    /// <summary> 
    /// Adds an order by descending expression for sorting the entities. 
    /// </summary> 
    /// <param name="orderByDescendingExpression">The order by descending expression to add.</param>
    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) =>
         OrderByDescendingExpression = orderByDescendingExpression;
}
