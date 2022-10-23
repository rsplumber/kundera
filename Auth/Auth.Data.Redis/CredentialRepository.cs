using Auth.Core;
using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;

namespace Auth.Data;

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

        return _mapper.Map<Credential>(dataModel);
    }

    public async Task DeleteAsync(UniqueIdentifier id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _credentials.FindByIdAsync(id.Value);

        if (dataModel is null) return;

        await _credentials.DeleteAsync(dataModel);
    }

    public async Task<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        return await _credentials.AnyAsync(model => model.Id == uniqueIdentifier.Value);
    }

    public async Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<CredentialDataModel>(entity);
        await _credentials.UpdateAsync(dataModel);
    }
}