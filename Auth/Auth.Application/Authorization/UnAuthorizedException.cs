﻿namespace Auth.Application.Authorization;

public class UnAuthorizedException : ApplicationException
{
    private const string DefaultMessage = "UnAuthorized";

    public UnAuthorizedException() : base(DefaultMessage)
    {
    }
}