using BookManagement.Application.Common.Data;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

internal sealed class GetUserByIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper
) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userDto = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == request.UserId)
            .ProjectToType<UserResponse>(mapper.Config)
            .SingleOrDefaultAsync(cancellationToken);

        if (userDto is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFound(request.UserId));
        }

        return Result.Success(userDto);
    }
}
