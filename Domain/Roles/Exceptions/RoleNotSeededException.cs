namespace Domain.Roles.Exceptions;

public class RoleNotSeededException : NotSupportedException
{
    private const string DefaultMessage = "'admin' role not seeded";

    public RoleNotSeededException() : base(DefaultMessage)
    {
    }
}