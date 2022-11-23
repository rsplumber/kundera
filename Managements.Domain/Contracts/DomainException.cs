namespace Managements.Domain.Contracts;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}