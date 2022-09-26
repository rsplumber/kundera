﻿using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Application;

public interface ITimePeriodicCredentialService : ICredentialRemoveService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, int expirationTimeInSeconds, CancellationToken cancellationToken = default);
}