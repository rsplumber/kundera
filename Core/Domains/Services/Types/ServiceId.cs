using ValueOf;

namespace Core.Domains.Services.Types;

public sealed class ServiceId : ValueOf<Guid, ServiceId>
{
    public static ServiceId Generate() => From(Guid.NewGuid());
}