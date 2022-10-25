using AutoMapper;
using Managements.Domain;
using Managements.Domain.Groups;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal class GroupRepository : IGroupRepository
{
    private readonly RedisCollection<GroupDataModel> _groups;
    private readonly IMapper _mapper;


    public GroupRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _mapper = mapper;
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
    }

    public async Task AddAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.InsertAsync(dataModel);
    }

    public async Task<Group?> FindAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FindByIdAsync(id.Value.ToString());
        return _mapper.Map<Group>(dataModel);
    }

    public async Task UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<GroupDataModel>(entity);
        await _groups.UpdateAsync(dataModel);
    }

    public Task DeleteAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Group>> FindAsync(IEnumerable<GroupId> ids, CancellationToken cancellationToken = default)
    {
        var rawGroupIds = ids.Select(id => id.Value.ToString());
        var dataModels = await _groups.FindByIdsAsync(rawGroupIds);

        return dataModels.Values.Select(model => _mapper.Map<Group>(model));
    }

    public async Task<Group?> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _groups.FirstOrDefaultAsync(model => model.Name == name.Value);

        return _mapper.Map<Group>(dataModel);
    }
}