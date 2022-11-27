namespace Core.Domains.Contracts;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(Type entity) : base($"{nameof(entity)} not found")
    {
    }
}