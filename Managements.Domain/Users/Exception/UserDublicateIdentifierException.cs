﻿using Kite.Domain.Contracts;

namespace Managements.Domain.Users.Exception;

public class UserDuplicateIdentifierException : DomainException
{
    public UserDuplicateIdentifierException(string identifier) : base($"User {identifier} exists")
    {
    }
}