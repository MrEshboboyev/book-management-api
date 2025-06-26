using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects.Books;

namespace BookManagement.Domain.Errors;

/// <summary> 
/// Defines and organizes domain-specific errors. 
/// </summary>
public static class DomainErrors
{
    #region User

    #region Entities

    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
                "User.NotFound",
                $"The user with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
                "Users.NotExist",
                $"There is no users");

        public static readonly Error InvalidCredentials = new(
               "User.InvalidCredentials",
               "The provided credentials are invalid");

        public static readonly Error InvalidRoleName = new(
            "User.InvalidRoleName",
            "The specified role is invalid.");

        public static readonly Func<int, Error> RoleNotAssigned = roleId => new Error(
            "User.RoleNotAssigned",
            $"The specified role with ID {roleId} is not assigned to the user.");
    }

    public static class Role
    {
        public static readonly Func<int, Error> NotFound = id => new Error(
            "Role.NotFound",
            $"The role with the identifier {id} was not found.");
    }

    #endregion

    #region Value Objects

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");
        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");
        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");
        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    #endregion

    #endregion

    #region Book

    #region Entities

    public static class Book
    {
        public static readonly Func<string, Error> AlreadyExists = titleName => new Error(
            "Book.AlreadyExists",
            $"The book with the title '{titleName}' already exists.");

        public static readonly Error InvalidData = new(
            "Book.InvalidData",
            "The book data provided is invalid.");

        public static readonly Func<BookId, Error> NotFound = id => new Error(
            "Book.NotFound",
            $"The book with the identifier {id.Value} was not found.");

        public static readonly Error AlreadyDeleted = new(
            "Book.AlreadyDeleted",
            "The book is already deleted.");
    }

    #endregion

    #region Value Objects

    public static class Author
    {
        public static readonly Error Empty = new(
            "Author.Empty",
            "Author name is empty");
        public static readonly Error TooLong = new(
            "Author.TooLong",
            "Author name is too long");
    }

    public static class PublicationYear
    {
        public static readonly Error Invalid = new(
            "PublicationYear.Invalid",
            "Publication year is invalid");
    }

    public static class Title
    {
        public static readonly Error Empty = new(
            "Title.Empty",
            "Title is empty");
        public static readonly Error TooLong = new(
            "Title.TooLong",
            "Title is too long");
    }

    #endregion

    #endregion
}

