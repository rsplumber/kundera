namespace Core.Auth.Credentials;

public interface IAuthenticationActivityRepository
{
    Task AddAsync(AuthenticationActivity authenticationActivity, CancellationToken cancellationToken = default);
}