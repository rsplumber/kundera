using Managements.Application.Permissions;
using Managements.Domain.Permissions.Exceptions;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Permissions;

internal sealed class PermissionQueryHandler : IQueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public PermissionQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async ValueTask<PermissionResponse> Handle(PermissionQuery query, CancellationToken cancellationToken)
    {
        var permission = await _permissions.FindByIdAsync(query.Permission.ToString());
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        return new PermissionResponse(permission.Id, permission.Name)
        {
            Meta = permission.Meta,
        };
    }
}