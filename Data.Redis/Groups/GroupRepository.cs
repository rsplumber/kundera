using AutoMapper;
using Core.Groups;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Groups;

internal class GroupRepository : IGroupRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<GroupDataModel> _groups;
    private readonly IMapper _mapper;


    public GroupRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _mapper = mapper;
        _eventBus = eventBus;
        _groups = (RedisCollection<GroupDataModel>)provider.RedisCollection<GroupDataModel>();
    }

    public async Task AddAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Group?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FindByIdAsync(id.ToString());
        return _mapper.Map<Group>(dataModel);
    }

    public async Task UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Group?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FirstOrDefaultAsync(model => model.Name == name);

        return _mapper.Map<Group>(dataModel);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}