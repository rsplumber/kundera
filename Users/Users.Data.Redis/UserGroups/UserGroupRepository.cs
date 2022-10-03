﻿using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;
using Users.Domain.UserGroups;

namespace Users.Data.Redis.UserGroups;

internal class UserGroupRepository : IUserGroupRepository
{
    private readonly RedisCollection<UserGroupDataModel> _userGroups;
    private readonly IMapper _mapper;


    public UserGroupRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _mapper = mapper;
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public async Task AddAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        var userGroup = _mapper.Map<UserGroupDataModel>(entity);
        await _userGroups.InsertAsync(userGroup);
    }

    public async Task<UserGroup?> FindAsync(UserGroupId id, CancellationToken cancellationToken = default)
    {
        var userGroupDataModel = await _userGroups.FindByIdAsync(id.Value.ToString());
        return _mapper.Map<UserGroup>(userGroupDataModel , options => options.Items["_name"] = "XXXX");
    }

    public async Task UpdateAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        var userGroup = _mapper.Map<UserGroupDataModel>(entity);
        await _userGroups.UpdateAsync(userGroup);
    }
}