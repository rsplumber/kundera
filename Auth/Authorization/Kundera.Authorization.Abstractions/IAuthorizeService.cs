namespace Kundera.Authorization;

public interface IAuthorizeService
{
    ValueTask<bool> AuthorizeAsync(string token, CancellationToken cancellationToken = default);
}