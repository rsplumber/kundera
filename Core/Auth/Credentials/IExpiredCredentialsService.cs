namespace Core.Auth.Credentials;

public interface IExpiredCredentialsService
{
    Task DeleteAsync(CancellationToken cancellationToken = default);
}