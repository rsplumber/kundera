namespace Core.Auth.Sessions;

public interface IExpiredSessionsService
{
    Task DeleteAsync(CancellationToken cancellationToken = default);
}