﻿namespace Core.Domains.Auth.Sessions;

public interface ISessionRepository
{
    Task AddAsync(Session entity, CancellationToken cancellationToken = default);

    Task<Session?> FindAsync(string token, CancellationToken cancellationToken = default);

    Task<Session?> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<List<Session>> FindByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(int afterExpireInMinutes = 1, CancellationToken cancellationToken = default);
}