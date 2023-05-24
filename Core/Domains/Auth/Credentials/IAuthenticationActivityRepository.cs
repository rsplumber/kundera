namespace Core.Domains.Auth.Credentials;

public interface IAuthenticationActivityRepository
{
    Task AddAsync(AuthenticationActivity authenticationActivity, CancellationToken cancellationToken = default);

    Task<AuthenticationActivity?> FindLastByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default);

    Task<List<AuthenticationActivity>> FindCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default);
    Task RemoveExpiredActivitiesAsync(CancellationToken cancellationToken = default);
}