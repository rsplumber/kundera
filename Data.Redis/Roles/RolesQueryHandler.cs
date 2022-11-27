using Application.Roles;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal sealed class RolesQueryHandler : IQueryHandler<RolesQuery, List<RolesResponse>>
{
    private IRedisCollection<RoleDataModel> _roles;

    public RolesQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async ValueTask<List<RolesResponse>> Handle(RolesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _roles = _roles.Where(model => model.Name.Contains(query.Name));
        }

        var rolesDataModel = await _roles.ToListAsync();

        return rolesDataModel.Select(model => new RolesResponse(model.Id, model.Name)).ToList();
    }
}