namespace Core.Auth.Sessions;

public interface IExpiredAuthorizationActivityService
{
    Task DeleteAsync(CancellationToken cancellationToken = default);
}