using Core.Domains.Roles.Exceptions;
using Managements.Data.Permissions;
using Mediator;
using Queries.Roles;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

public sealed class RolePermissionsQueryHandler : IQueryHandler<RolePermissionsQuery, List<RolePermissionsResponse>>
{
    private readonly IRedisCollection<RoleDataModel> _roles;
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public RolePermissionsQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>)provider.RedisCollection<RoleDataModel>(false);
        _permissions = (RedisCollection<PermissionDataModel>)provider.RedisCollection<PermissionDataModel>(false);
    }

    public async ValueTask<List<RolePermissionsResponse>> Handle(RolePermissionsQuery query, CancellationToken cancellationToken)
    {
        var roleDataModel = await _roles.FindByIdAsync(query.RoleId.ToString());
        if (roleDataModel is null)
        {
            throw new RoleNotFoundException();
        }

        if (roleDataModel.Permissions is null)
        {
            return Array.Empty<RolePermissionsResponse>().ToList();
        }

        var permissionDataModels = await _permissions.FindByIdsAsync(roleDataModel.Permissions.Select(guid => guid.ToString()));

        return permissionDataModels.Values.Select(model => new RolePermissionsResponse(model!.Id, model.Name))
            .ToList();
    }
}