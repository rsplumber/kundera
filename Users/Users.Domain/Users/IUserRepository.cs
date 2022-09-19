using Tes.Domain.Contracts;
using Users.Domain.Users.Types;

namespace Users.Domain.Users;

public interface IUserRepository : IRepository<UserId, User>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);
}