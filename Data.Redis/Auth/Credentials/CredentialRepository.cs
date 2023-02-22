using AutoMapper;
using Core.Domains.Auth.Credentials;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Auth.Credentials;

internal class CredentialRepository : ICredentialRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<CredentialDataModel> _credentials;
    private readonly IMapper _mapper;

    public CredentialRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _credentials = (RedisCollection<CredentialDataModel>)provider.RedisCollection<CredentialDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Credential credential, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<CredentialDataModel>(credential);
        await _credentials.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(credential);
    }

    public async Task<Credential?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.ToString());
        return _mapper.Map<Credential>(dataModel);
    }

    public async Task<List<Credential>> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var credentials = await _credentials.Where(model => model.Username == username).ToListAsync();
        return credentials.Select(model => _mapper.Map<Credential>(model)).ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.ToString());
        if (dataModel is null) return;
        await _credentials.DeleteAsync(dataModel);
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var credentials = await _credentials.ToListAsync();
        var expiredCredentials = credentials
            .Where(model => model.ExpiresAtUtc is not null)
            .Where(model => DateTime.UtcNow >= model.ExpiresAtUtc!.Value.ToUniversalTime());

        foreach (var credential in expiredCredentials)
        {
            await _credentials.DeleteAsync(credential);
        }
    }

    public async Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<CredentialDataModel>(entity);
        await _credentials.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }
}