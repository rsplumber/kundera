using Application.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Roles;

public sealed class RoleQueryHandler : IQueryHandler<RoleQuery, RoleResponse>
{
    private readonly IRedisCollection<RoleDataModel> _roles;

    public RoleQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>)provider.RedisCollection<RoleDataModel>(false);
    }

    public async ValueTask<RoleResponse> Handle(RoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _roles.FindByIdAsync(query.RoleId.ToString());
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Meta = role.Meta,
            Permissions = role.Permissions
        };
    }
}