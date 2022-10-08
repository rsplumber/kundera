using Auth.Domain.Credentials;
using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;

namespace Authentication.Data.Redis;

internal class CredentialRepository : ICredentialRepository
{
    private readonly RedisCollection<CredentialDataModel> _credentials;
    private readonly IMapper _mapper;

    public CredentialRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _credentials = (RedisCollection<CredentialDataModel>) provider.RedisCollection<CredentialDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Credential entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<CredentialDataModel>(entity);
        if (entity.ExpiresAt is not null)
        {
            await _credentials.InsertAsync(dataModel, TimeSpan.FromTicks(entity.ExpiresAt.Value.Ticks));
        }
        else
        {
            await _credentials.InsertAsync(dataModel);
        }
    }

    public async Task<Credential?> FindAsync(UniqueIdentifier id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.Value);
        var credential = _mapper.Map<Credential>(dataModel);
        if (credential is null) return null;
        if (credential.OneTime)
        {
            await DeleteAsync(credential.Id, cancellationToken);
        }

        return credential;
    }

    public async Task DeleteAsync(UniqueIdentifier id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.Value);
        if (dataModel is null) return;
        await _credentials.DeleteAsync(dataModel);
    }

    public async ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        return await _credentials.AnyAsync(model => model.Id == uniqueIdentifier.Value);
    }

    public async Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<CredentialDataModel>(entity);
        await _credentials.UpdateAsync(dataModel);
    }
}