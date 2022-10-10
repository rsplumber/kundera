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
        await _credentials.InsertAsync(dataModel);
    }

    public async Task<Credential?> FindAsync(UniqueIdentifier id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.Value);
        if (dataModel is null) return null;

        if (Expired())
        {
            await DeleteAsync(id, cancellationToken);
            return null;
        }

        if (dataModel.OneTime)
        {
            await DeleteAsync(id, cancellationToken);
        }

        return _mapper.Map<Credential>(dataModel);

        bool Expired() => DateTime.UtcNow >= dataModel.ExpiresAt;
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