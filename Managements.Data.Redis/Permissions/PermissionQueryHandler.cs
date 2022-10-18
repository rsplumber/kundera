using Kite.CQRS;
using Managements.Application.Permissions;
using Managements.Domain.Permissions.Exceptions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Permissions;

internal sealed class PermissionQueryHandler : IQueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public PermissionQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async Task<PermissionResponse> HandleAsync(PermissionQuery message, CancellationToken cancellationToken = default)
    {
        var permission = await _permissions.FindByIdAsync(message.Permission.Value);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        return new PermissionResponse(permission.Id)
        {
            Meta = permission.Meta,
        };
    }
}