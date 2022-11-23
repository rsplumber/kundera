using Managements.Application.Roles;
using Managements.Domain.Roles.Exceptions;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal sealed class RoleQueryHandler : IQueryHandler<RoleQuery, RoleResponse>
{
    private readonly IRedisCollection<RoleDataModel> _roles;

    public RoleQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async ValueTask<RoleResponse> Handle(RoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _roles.FindByIdAsync(query.Role.ToString());
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new RoleResponse(role.Id, role.Name)
        {
            Meta = role.Meta,
            Permissions = role.Permissions
        };
    }
}