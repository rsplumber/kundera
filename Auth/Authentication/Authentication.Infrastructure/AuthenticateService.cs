﻿using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;
using Authorization.Application;

namespace Authentication.Infrastructure;

internal class AuthenticateService : IAuthenticateService
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly ICertificateService _certificateService;
    private readonly ISessionManagement _sessionManagement;

    public AuthenticateService(ICredentialRepository credentialRepository,
        ICertificateService certificateService
        , ISessionManagement sessionManagement)
    {
        _credentialRepository = credentialRepository;
        _certificateService = certificateService;
        _sessionManagement = sessionManagement;
    }

    public async Task<Certificate> AuthenticateAsync(UniqueIdentifier uniqueIdentifier, Password password, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new UnAuthenticateException();
        }

        if (credential.Password != password)
        {
            throw new UnAuthenticateException();
        }

        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        var certificate = await _certificateService.GenerateAsync(credential.User.ToString(), cancellationToken);
        await _sessionManagement.SaveAsync(certificate, "global", ipAddress ?? IPAddress.None, cancellationToken);
        return certificate;
    }
}