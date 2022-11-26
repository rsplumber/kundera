﻿using Application.Groups;
using Managements.Data.ConnectionProviders;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, IEnumerable<GroupsResponse>>
{
    private readonly IRedisCollection<GroupDataModel> _groups;

    public GroupsQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
    }

    public async ValueTask<IEnumerable<GroupsResponse>> Handle(GroupsQuery query, CancellationToken cancellationToken)
    {
        var groups = await _groups.ToListAsync();

        return groups.Select(model => new GroupsResponse(model.Id, model.Name, model.Status)
            {
                Description = model.Description,
                Parent = model.Parent,
            })
            .ToList();
    }
}