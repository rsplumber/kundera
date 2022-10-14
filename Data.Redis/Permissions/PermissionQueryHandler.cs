using Application.Permissions;
using Domain.Permissions.Exceptions;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Permissions;

internal sealed class PermissionIQueryHandler : IQueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public PermissionIQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async ValueTask<PermissionResponse> HandleAsync(PermissionQuery message, CancellationToken cancellationToken = default)
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