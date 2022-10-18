using Kite.CQRS;
using Managements.Application.Permissions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Permissions;

internal sealed class PermissionsQueryHandler : IQueryHandler<PermissionsQuery, IEnumerable<PermissionsResponse>>
{
    private IRedisCollection<PermissionDataModel> _permissions;

    public PermissionsQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async Task<IEnumerable<PermissionsResponse>> HandleAsync(PermissionsQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _permissions = _permissions.Where(model => model.Name.Contains(message.Name));
        }

        var permissionsDataModel = await _permissions.ToListAsync();

        return permissionsDataModel.Select(model => new PermissionsResponse(model.Id, model.Name));
    }
}