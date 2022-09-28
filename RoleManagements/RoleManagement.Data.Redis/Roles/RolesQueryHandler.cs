using Redis.OM;
using Redis.OM.Searching;
using RoleManagement.Application.Roles;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Roles;

internal sealed class RolesQueryHandler : QueryHandler<RolesQuery, IEnumerable<RolesResponse>>
{
    private IRedisCollection<RoleDataModel> _roles;

    public RolesQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public override async Task<IEnumerable<RolesResponse>> HandleAsync(RolesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _roles = _roles.Where(model => model.Id.Contains(message.Name));
        }

        var rolesDataModel = await _roles.ToListAsync();

        return rolesDataModel.Select(model => new RolesResponse(model.Id));
    }
}