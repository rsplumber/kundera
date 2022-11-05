using Kite.CQRS;
using Managements.Application.Roles;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal sealed class RolesQueryHandler : IQueryHandler<RolesQuery, IEnumerable<RolesResponse>>
{
    private IRedisCollection<RoleDataModel> _roles;

    public RolesQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async Task<IEnumerable<RolesResponse>> HandleAsync(RolesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _roles = _roles.Where(model => model.Name.Contains(message.Name));
        }

        var rolesDataModel = await _roles.ToListAsync();

        return rolesDataModel.Select(model => new RolesResponse(model.Id, model.Name));
    }
}