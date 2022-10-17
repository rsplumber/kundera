using Kite.CustomType;

namespace Managements.Domain.Services.Types;

public class ServiceStatus : CustomType<string, ServiceStatus>
{
    public static readonly ServiceStatus Active = From(nameof(Active));
    public static readonly ServiceStatus DeActive = From(nameof(DeActive));

    protected override void Validate()
    {
        if (Value is not (nameof(Active) or nameof(DeActive)))
        {
            throw new ServiceStatusNotSupportedException(Value);
        }

        base.Validate();
    }
}