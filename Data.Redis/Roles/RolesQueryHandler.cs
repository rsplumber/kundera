﻿using Application.Roles;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Roles;

internal sealed class RolesIQueryHandler : IQueryHandler<RolesQuery, IEnumerable<RolesResponse>>
{
    private IRedisCollection<RoleDataModel> _roles;

    public RolesIQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async ValueTask<IEnumerable<RolesResponse>> HandleAsync(RolesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _roles = _roles.Where(model => model.Id.Contains(message.Name));
        }

        var rolesDataModel = await _roles.ToListAsync();

        return rolesDataModel.Select(model => new RolesResponse(model.Id));
    }
}