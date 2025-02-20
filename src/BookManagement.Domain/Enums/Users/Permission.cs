namespace BookManagement.Domain.Enums.Users;

/// <summary>
/// Represents different permissions that can be assigned to roles.
/// </summary>
public enum Permission
{
    ReadUser = 1,
    UpdateUser = 2,
    AddBook = 3,
    AddBooksBulk = 4,
    UpdateBook = 5,
    SoftDeleteBook = 6,
    SoftDeleteBooksBulk = 7,
    GetBookDetails = 8,
    GetBooksList = 9
}
