﻿using System.Net;
using Authorization.Domain.Types;

namespace Authorization.Application;

public interface ISessionManagement
{
    Task SaveAsync(Certificate certificate, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token token, CancellationToken cancellationToken = default);

    Task<SessionModel?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default);

    Task<IEnumerable<SessionModel>> GetAllAsync(CancellationToken cancellationToken = default);
}