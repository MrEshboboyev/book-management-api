using BookManagement.Domain.Entities.Users;
using Mapster;

namespace BookManagement.Application.Users.Queries.Common.Responses;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName);

public class UserMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>();
            //.Map(dest => dest.Email, src => src.Email.Value)
            //.Map(dest => dest.FirstName, src => src.FirstName.Value)
            //.Map(dest => dest.LastName, src => src.LastName.Value);
    }
}
