using Core.Domains.Contracts;

namespace Core.Domains.Services.Types;

public sealed class ServiceStatus : Enumeration
{
    public static readonly ServiceStatus Active = new(1, nameof(Active));
    public static readonly ServiceStatus DeActive = new(2, nameof(DeActive));

    private ServiceStatus(int id, string name) : base(id, name)
    {
    }
}