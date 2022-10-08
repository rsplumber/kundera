using Application.Permissions;
using Domain.Permissions.Exceptions;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Permissions;

internal sealed class PermissionQueryHandler : QueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public PermissionQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public override async Task<PermissionResponse> HandleAsync(PermissionQuery message, CancellationToken cancellationToken = default)
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