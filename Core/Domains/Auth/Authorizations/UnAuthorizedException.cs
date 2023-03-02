﻿namespace Core.Domains.Auth.Authorizations;

public sealed class UnAuthorizedException : KunderaException
{
    private const int DefaultCode = 401;
    private const string DefaultMessage = "Unauthorized";

    public UnAuthorizedException() : base(DefaultCode, DefaultMessage)
    {
    }
}