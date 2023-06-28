namespace Core.Auth.Credentials;

public interface IExpiredAuthenticationActivityService
{
    Task DeleteAsync(CancellationToken cancellationToken = default);
}