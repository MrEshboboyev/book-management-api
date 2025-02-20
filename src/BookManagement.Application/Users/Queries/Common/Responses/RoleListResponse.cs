namespace BookManagement.Application.Users.Queries.Common.Responses;

public sealed record RoleListResponse(IReadOnlyCollection<RoleResponse> Roles);