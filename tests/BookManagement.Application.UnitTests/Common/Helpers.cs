using BookManagement.Domain.Entities.Books;
using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Identity.Books;
using BookManagement.Domain.ValueObjects.Books;
using BookManagement.Domain.ValueObjects.Users;

namespace BookManagement.Application.UnitTests.Common;

public static class Helpers
{
    // Helper method to create a User instance
    public static User CreateTestUser(Guid id,
                                      string email,
                                      string passwordHash,
                                      string firstName,
                                      string lastName)
    {
        var emailObj = Email.Create(email).Value;
        var firstNameObj = FirstName.Create(firstName).Value;
        var lastNameObj = LastName.Create(lastName).Value;
        var role = Role.Registered;

        return User.Create(id, emailObj, passwordHash, firstNameObj, lastNameObj, role);
    }

    // Helper method to create a Book instance
    public static Book CreateTestBook(
                                      string title,
                                      int publicationYear,
                                      string author)
    {
        var titleObj = Title.Create(title).Value;
        var authorObj = Author.Create(author).Value;
        var publicationYearObj = PublicationYear.Create(publicationYear).Value;

        return Book.Create(titleObj, publicationYearObj, authorObj).Value;
    }
}
