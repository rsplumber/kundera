using AutoMapper;
using DotNetCore.CAP;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Types;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal class GroupRepository : IGroupRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<GroupDataModel> _groups;
    private readonly IMapper _mapper;


    public GroupRepository(RedisConnectionManagementsProviderWrapper provider, IMapper mapper, ICapPublisher eventBus)
    {
        _mapper = mapper;
        _eventBus = eventBus;
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>(false);
    }

    public async Task AddAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Group?> FindAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FindByIdAsync(id.Value.ToString());
        return _mapper.Map<Group>(dataModel);
    }

    public async Task UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<IEnumerable<Group>> FindAsync(IEnumerable<GroupId> ids, CancellationToken cancellationToken = default)
    {
        var dataModels = await _groups.FindByIdsAsync(ids.Select(id => id.Value.ToString()));
        return dataModels.Values.Select(model => _mapper.Map<Group>(model));
    }

    public async Task<IEnumerable<Group>> FindChildrenAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        var currentGroup = await _groups.FindByIdAsync(id.Value.ToString());
        if (currentGroup is null) return Array.Empty<Group>();
        var dataModels = new List<GroupDataModel>();
        await FetchChildrenAsync(currentGroup);

        return dataModels.Select(model => _mapper.Map<Group>(model));

        async Task FetchChildrenAsync(GroupDataModel group)
        {
            if (group.Children.Count == 0) return;
            var ids = group.Children.Select(groupId => groupId.ToString()).ToArray();
            var children = (await _groups.FindByIdsAsync(ids)).Values;
            dataModels.AddRange(children!);
            foreach (var groupDataModel in children)
            {
                await FetchChildrenAsync(groupDataModel!);
            }
        }
    }

    public async Task<Group?> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FirstOrDefaultAsync(model => model.Name == name.Value);

        return _mapper.Map<Group>(dataModel);
    }

    public async Task DeleteAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}