using Mediator;
using Queries;
using Queries.Roles;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Roles;

public sealed class RolesQueryHandler : IQueryHandler<RolesQuery, PageableResponse<RolesResponse>>
{
    private IRedisCollection<RoleDataModel> _roles;

    public RolesQueryHandler(RedisConnectionProvider provider)
    {
        _roles = (RedisCollection<RoleDataModel>)provider.RedisCollection<RoleDataModel>(false);
    }

    public async ValueTask<PageableResponse<RolesResponse>> Handle(RolesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _roles = _roles.Where(model => model.Name.Contains(query.Name));
        }

        var rolesQuery = await _roles.Page(query).ToListAsync();
        var roles = rolesQuery.Select(model => new RolesResponse(model.Id, model.Name)).ToList();
        var counts = 0;
        if (query.Name is not null)
        {
            counts = await _roles.Where(model => model.Name.Contains(query.Name)).CountAsync();
        }
        else
        {
            counts = await _roles.CountAsync();
        }

        return new PageableResponse<RolesResponse>
        {
            Data = roles,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}