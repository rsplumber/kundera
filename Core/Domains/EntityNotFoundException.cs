namespace Core.Domains;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(Type entity) : base($"{nameof(entity)} not found")
    {
    }
}