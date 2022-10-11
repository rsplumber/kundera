namespace Domain.Services.Exceptions;

public class ServiceNotSeededException : NotSupportedException
{
    private const string DefaultMessage = "'Kundera' Service not seeded";
    public ServiceNotSeededException() : base(DefaultMessage)
    {
    }
}