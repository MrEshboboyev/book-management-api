using BookManagement.Domain.Entities.Users;
using Mapster;

namespace BookManagement.Application.Users.Queries.Common.Responses;

public sealed record RoleResponse(
    int RoleId,
    string RoleName);

public class RoleMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Role, RoleResponse>()
            .Map(dest => dest.RoleId, src => src.Id)
            .Map(dest => dest.RoleName, src => src.Name);
    }
}
