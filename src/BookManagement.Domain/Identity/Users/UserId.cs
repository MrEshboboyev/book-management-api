using BookManagement.Domain.Primitives.Id;

namespace BookManagement.Domain.Identity.Users;

public sealed class UserId : StronglyTypedId<GlobalId>
{
    public UserId(GlobalId value) : base(value) { }

    public static UserId New() => new(GlobalId.New());
}
