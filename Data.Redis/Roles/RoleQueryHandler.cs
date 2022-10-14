﻿using Application.Roles;
using Domain.Roles;
using Domain.Roles.Exceptions;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Roles;

internal sealed class RoleQueryHandler : IQueryHandler<RoleQuery, RoleResponse>
{
    private readonly IRedisCollection<RoleDataModel> _roles;
    private readonly IRoleRepository _roleRepository;

    public RoleQueryHandler(RedisConnectionProvider provider, IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async ValueTask<RoleResponse> HandleAsync(RoleQuery message, CancellationToken cancellationToken = default)
    {
        var roles = await _roleRepository.FindAsync(message.RoleId, cancellationToken);
        var role = await _roles.FindByIdAsync(message.RoleId.Value);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new RoleResponse(role.Id)
        {
            Meta = role.Meta,
            Permissions = role.Permissions
        };
    }
}