namespace Core.Domains.Contracts;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}