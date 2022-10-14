﻿using Kite.Domain.Contracts;

namespace Domain.Users.Exception;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException() : base("Email Format Is Invalid")
    {
    }
}