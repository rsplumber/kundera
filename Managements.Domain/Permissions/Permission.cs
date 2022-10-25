using Kite.Domain.Contracts;
using Managements.Domain.Permissions.Events;

namespace Managements.Domain.Permissions;

public class Permission : AggregateRoot<PermissionId>
{
    protected Permission()
    {
    }

    internal Permission(Name name) : base(PermissionId.Generate())
    {
        Name = name;
        AddDomainEvent(new PermissionCreatedEvent(Id));
    }

    public Name Name { get; internal set; }

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(Name name) => Name = name;
}