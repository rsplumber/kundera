using Redis.OM;
using Redis.OM.Searching;
using Users.Domain.UserGroups;

namespace Users.Data.Redis.UserGroups;

internal class UserGroupRepository : IUserGroupRepository
{
    private readonly RedisConnectionProvider _provider;
    private readonly RedisCollection<UserGroup> _userGroup;


    public UserGroupRepository(RedisConnectionProvider provider)
    {
        _provider = provider;
        _userGroup = (RedisCollection<UserGroup>) provider.RedisCollection<UserGroup>();
    }

    public async Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default)
    {
        await _userGroup.InsertAsync(userGroup);
    }

    public async Task<UserGroup?> FindAsync(UserGroupId id, CancellationToken cancellationToken = default)
    {
        return await _userGroup.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(UserGroup userGroup, CancellationToken cancellationToken = default)
    {
        await _userGroup.UpdateAsync(userGroup);
    }
}