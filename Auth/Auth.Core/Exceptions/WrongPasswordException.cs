using Managements.Domain.Contracts;

namespace Auth.Core.Exceptions;

public sealed class WrongPasswordException : DomainException
{
    private const string DefaultMessage = "wrong password";

    public WrongPasswordException() : base(DefaultMessage)
    {
    }
}