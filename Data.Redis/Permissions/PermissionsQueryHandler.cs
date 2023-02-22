using Mediator;
using Queries.Permissions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Permissions;

internal sealed class PermissionsQueryHandler : IQueryHandler<PermissionsQuery, List<PermissionsResponse>>
{
    private IRedisCollection<PermissionDataModel> _permissions;

    public PermissionsQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>)provider.RedisCollection<PermissionDataModel>(false);
    }

    public async ValueTask<List<PermissionsResponse>> Handle(PermissionsQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _permissions = _permissions.Where(model => model.Name.Contains(query.Name));
        }

        var permissionsDataModel = await _permissions.ToListAsync();

        return permissionsDataModel.Select(model => new PermissionsResponse(model.Id, model.Name)).ToList();
    }
}