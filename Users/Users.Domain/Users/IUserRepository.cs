using Tes.Domain.Contracts;
using Users.Domain.Users.Types;

namespace Users.Domain.Users;

public interface IUserRepository : IRepository<UserId, User>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsAsync(Email email, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsAsync(NationalCode nationalCode, CancellationToken cancellationToken = default);
}