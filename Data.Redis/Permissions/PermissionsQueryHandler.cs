using Application.Permissions;
using Managements.Data.ConnectionProviders;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Permissions;

internal sealed class PermissionsQueryHandler : IQueryHandler<PermissionsQuery, IEnumerable<PermissionsResponse>>
{
    private IRedisCollection<PermissionDataModel> _permissions;

    public PermissionsQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async ValueTask<IEnumerable<PermissionsResponse>> Handle(PermissionsQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _permissions = _permissions.Where(model => model.Name.Contains(query.Name));
        }

        var permissionsDataModel = await _permissions.ToListAsync();

        return permissionsDataModel.Select(model => new PermissionsResponse(model.Id, model.Name));
    }
}