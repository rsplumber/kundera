﻿namespace Core.Domains.Users.Exception;

public sealed class UsernameCouldNotBeEmptyException : DomainException
{
    private const string DefaultMessage = "Username could not be empty";

    public UsernameCouldNotBeEmptyException() : base(DefaultMessage)
    {
    }
}