namespace Core.Domains.Auth.Sessions;

public interface IAuthorizationActivityRepository
{
    Task AddAsync(AuthorizationActivity authenticationActivity, CancellationToken cancellationToken = default);

    Task<AuthorizationActivity?> FindLastBySessionAsync(string sessionId, CancellationToken cancellationToken = default);

    Task<List<AuthorizationActivity>> FindBySessionAsync(string sessionId, CancellationToken cancellationToken = default);

    Task RemoveExpiredActivitiesAsync(CancellationToken cancellationToken = default);
}