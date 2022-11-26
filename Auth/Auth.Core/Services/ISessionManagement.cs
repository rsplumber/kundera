﻿using Auth.Core.Entities;

namespace Auth.Core.Services;

public interface ISessionManagement
{
    Task SaveAsync(Certificate certificate, Guid userId, Guid scopeId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token token, CancellationToken cancellationToken = default);

    Task<Session?> GetAsync(Token token, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
}