﻿using AutoMapper;
using Kite.Events;
using Managements.Domain.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

internal class UserRepository : IUserRepository
{
    private readonly IEventBus _eventBus;
    private readonly RedisCollection<UserDataModel> _users;
    private readonly IMapper _mapper;

    public UserRepository(RedisConnectionProvider provider, IMapper mapper, IEventBus eventBus)
    {
        _mapper = mapper;
        _eventBus = eventBus;
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<UserDataModel>(entity);
        await _users.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _users.FindByIdAsync(id.Value.ToString());
        return _mapper.Map<User>(dataModel);
    }

    public async Task<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(model => model.Usernames.Contains(username.Value));
    }

    public async Task<User?> FindAsync(Username username, CancellationToken cancellationToken = default)
    {
        var dataModel = await _users.Where(model => model.Usernames.Contains(username))
            .FirstOrDefaultAsync();
        return _mapper.Map<User>(dataModel);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<UserDataModel>(entity);
        await _users.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}