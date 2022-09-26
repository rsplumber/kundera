using Redis.OM;
using Redis.OM.Searching;
using RoleManagement.Application.Roles;
using RoleManagements.Domain.Roles.Exceptions;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Roles;

internal sealed class RoleQueryHandler : QueryHandler<RoleQuery, RoleResponse>
{
    private readonly IRedisCollection<RoleDataModel> _roles;

    public RoleQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public override async Task<RoleResponse> HandleAsync(RoleQuery message, CancellationToken cancellationToken = default)
    {
        var role = await _roles.FindByIdAsync(message.RoleId.Value);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new RoleResponse(role.Id)
        {
            Meta = role.Meta
        };
    }
}