using Kite.CQRS;
using Managements.Application.Roles;
using Managements.Domain.Roles.Exceptions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Roles;

internal sealed class RoleQueryHandler : IQueryHandler<RoleQuery, RoleResponse>
{
    private readonly IRedisCollection<RoleDataModel> _roles;

    public RoleQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async Task<RoleResponse> HandleAsync(RoleQuery message, CancellationToken cancellationToken = default)
    {
        var role = await _roles.FindByIdAsync(message.RoleId.ToString());
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