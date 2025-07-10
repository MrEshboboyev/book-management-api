using BookManagement.Domain.Errors;
using BookManagement.Domain.Events.Users;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Users;

namespace BookManagement.Domain.Entities.Users;

/// <summary> 
/// Represents a user in the system. 
/// </summary>
public sealed class User : AggregateRoot, IAuditableEntity
{
    #region Private fields

    private readonly List<Role> _roles = [];

    #endregion

    #region Constructors 

    private User(
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName): base()
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
    }

    private User()
    {
    }
    
    #endregion

    #region Properties
    
    public string PasswordHash { get; set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public Email Email { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    
    #endregion
    
    #region Factory Methods

    /// <summary> 
    /// Creates a new user instance. 
    /// </summary>
    public static User Create(
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName,
        Role role)
    {
        #region Create new User
        
        var user = new User(
            email,
            passwordHash,
            firstName,
            lastName);

        #endregion

        user.AssignRole(role);

        #region Domain Events

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(
            Guid.NewGuid(),
            user.Id));
        
        #endregion

        return user;
    }
    
    #endregion

    #region Own Methods
    
    /// <summary> 
    /// Changes the user's name and raises a domain event if the name has changed. 
    /// </summary>
    public void ChangeName(
        FirstName firstName,
        LastName lastName)
    {
        #region Checking new values are equals old valus
        
        if (!FirstName.Equals(firstName) || !LastName.Equals(lastName))
        {
            RaiseDomainEvent(new UserNameChangedDomainEvent(
                Guid.NewGuid(),
                Id));
        }
        
        #endregion

        #region Update fields
        
        FirstName = firstName;
        LastName = lastName;
        
        #endregion
    }

    #endregion

    #region Role related

    public Result AssignRole(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.Name))
        {
            return Result.Failure(
                DomainErrors.User.InvalidRoleName);
        }

        if (!IsInRole(role))
            _roles.Add(role);

        return Result.Success();
    }

    public Result RemoveRole(Role role)
    {
        #region Validate role

        if (string.IsNullOrWhiteSpace(role.Name))
        {
            return Result.Failure(
                DomainErrors.User.InvalidRoleName);
        }

        if (!IsInRole(role))
        {
            return Result.Failure(
                DomainErrors.User.RoleNotAssigned(role.Id));
        }

        #endregion

        #region Remove role

        _roles.Remove(role);

        #endregion

        return Result.Success();
    }

    public bool IsInRole(Role role) => _roles.Contains(role);

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = FirstName.Create(firstName).Value;
        LastName = LastName.Create(lastName).Value;
    }

    #endregion
}