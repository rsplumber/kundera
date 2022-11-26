﻿using Application.Roles;
using Core.Domains.Roles.Exceptions;
using Managements.Data.ConnectionProviders;
using Managements.Data.Permissions;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal sealed class RolePermissionsQueryHandler : IQueryHandler<RolePermissionsQuery, IEnumerable<RolePermissionsResponse>>
{
    private readonly IRedisCollection<RoleDataModel> _roles;
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public RolePermissionsQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
    }

    public async ValueTask<IEnumerable<RolePermissionsResponse>> Handle(RolePermissionsQuery query, CancellationToken cancellationToken)
    {
        var roleDataModel = await _roles.FindByIdAsync(query.Role.ToString());
        if (roleDataModel is null)
        {
            throw new RoleNotFoundException();
        }

        var permissionDataModels = await _permissions.FindByIdsAsync(roleDataModel.Permissions.Select(guid => guid.ToString()));

        return permissionDataModels.Values.Select(model => new RolePermissionsResponse(model!.Id, model.Name));
    }
}