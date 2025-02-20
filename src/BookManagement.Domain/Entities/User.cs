using BookManagement.Domain.Events;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.ValueObjects;

namespace BookManagement.Domain.Entities;

/// <summary> 
/// Represents a user in the system. 
/// </summary>
public sealed class User : AggregateRoot, IAuditableEntity
{
    #region Constructors 
    
    private User(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName): base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
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
    public ICollection<Role> Roles { get; set; }
    
    #endregion
    
    #region Factory Methods

    /// <summary> 
    /// Creates a new user instance. 
    /// </summary>
    public static User Create(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName
        )
    {
        #region Create new User
        
        var user = new User(
            id,
            email,
            passwordHash,
            firstName,
            lastName);
        
        #endregion
        
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
}

