using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Queries.GetAllRoles;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, RoleListResponse>
{
    public async Task<Result<RoleListResponse>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        // Fetch all users from the repository
        var users = await roleRepository.GetAllAsync(cancellationToken);

        #region Prepare response
        
        var responses = new RoleListResponse(
            users
                .Select(RoleResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion

        return Result.Success(responses);
    }
}
