namespace Kundera.Authorization;

public interface ICurrentUserService
{
    Guid UserId();

    bool IsAuthenticated();
}