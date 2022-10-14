using Application.Permissions;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Permissions;

internal sealed class PermissionsIQueryHandler : IQueryHandler<PermissionsQuery, IEnumerable<PermissionsResponse>>
{
    private IRedisCollection<PermissionDataModel> _permissions;

    public PermissionsIQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async ValueTask<IEnumerable<PermissionsResponse>> HandleAsync(PermissionsQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _permissions = _permissions.Where(model => model.Id.Contains(message.Name));
        }

        var permissionsDataModel = await _permissions.ToListAsync();

        return permissionsDataModel.Select(model => new PermissionsResponse(model.Id));
    }
}