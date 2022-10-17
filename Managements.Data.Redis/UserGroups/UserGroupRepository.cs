using AutoMapper;
using Managements.Domain;
using Managements.Domain.UserGroups;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.UserGroups;

internal class UserGroupRepository : IUserGroupRepository
{
    private readonly RedisCollection<UserGroupDataModel> _userGroups;
    private readonly IMapper _mapper;


    public UserGroupRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _mapper = mapper;
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public async ValueTask AddAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        var userGroup = _mapper.Map<UserGroupDataModel>(entity);
        await _userGroups.InsertAsync(userGroup);
    }

    public async ValueTask<UserGroup?> FindAsync(UserGroupId id, CancellationToken cancellationToken = default)
    {
        var userGroupDataModel = await _userGroups.FindByIdAsync(id.Value.ToString());

        return _mapper.Map<UserGroup>(userGroupDataModel);
    }

    public async ValueTask UpdateAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        var userGroup = _mapper.Map<UserGroupDataModel>(entity);
        await _userGroups.UpdateAsync(userGroup);
    }

    public ValueTask DeleteAsync(UserGroupId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<IEnumerable<UserGroup>> FindAsync(UserGroupId[] groupIds, CancellationToken cancellationToken = default)
    {
        var rawGroupIds = groupIds.Select(id => id.Value.ToString());
        var dataModels = await _userGroups.FindByIdsAsync(rawGroupIds);

        return dataModels.Values.Select(model => _mapper.Map<UserGroup>(model));
    }

    public async ValueTask<UserGroup?> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var userGroupDataModel = await _userGroups.FirstOrDefaultAsync(model => model.Name == name);

        return _mapper.Map<UserGroup>(userGroupDataModel);
    }
}