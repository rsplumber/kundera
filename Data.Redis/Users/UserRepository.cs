using AutoMapper;
using Core.Domains.Users;
using Core.Domains.Users.Types;
using DotNetCore.CAP;
using Managements.Data.ConnectionProviders;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

internal class UserRepository : IUserRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<UserDataModel> _users;
    private readonly IMapper _mapper;

    public UserRepository(RedisConnectionManagementsProviderWrapper provider, IMapper mapper, ICapPublisher eventBus)
    {
        _mapper = mapper;
        _eventBus = eventBus;
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>(false);
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