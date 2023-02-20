using Application.Permissions;
using Core.Domains.Permissions.Exceptions;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Permissions;

internal sealed class PermissionQueryHandler : IQueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly IRedisCollection<PermissionDataModel> _permissions;

    public PermissionQueryHandler(RedisConnectionProvider provider)
    {
        _permissions = (RedisCollection<PermissionDataModel>)provider.RedisCollection<PermissionDataModel>(false);
    }

    public async ValueTask<PermissionResponse> Handle(PermissionQuery query, CancellationToken cancellationToken)
    {
        var permission = await _permissions.FindByIdAsync(query.PermissionId.ToString());
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        return new PermissionResponse
        {
            Id = permission.Id,
            Name = permission.Name,
            Meta = permission.Meta,
        };
    }
}