﻿using Tes.Domain.Contracts;

namespace Domain.Users;

public interface IUserRepository : IRepository<UserId, User>, IUpdateService<User>, IDeleteService<UserId>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User> FindAsync(Username username, CancellationToken cancellationToken = default);
}