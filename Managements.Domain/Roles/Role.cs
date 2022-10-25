using Kite.Domain.Contracts;
using Managements.Domain.Permissions;
using Managements.Domain.Roles.Events;

namespace Managements.Domain.Roles;

public class Role : AggregateRoot<RoleId>
{
    protected Role()
    {
    }

    internal Role(Name name) : base(RoleId.Generate())
    {
        Name = name;
        AddDomainEvent(new RoleCreatedEvent(Id));
    }


    public Name Name { get; internal set; }
    
    public IReadOnlyCollection<PermissionId> Permissions { get; internal set; } = new List<PermissionId>();

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(Name name) => Name = name;

    public void AddPermission(PermissionId permission)
    {
        if (Has(permission)) return;

        var modifiablePermissions = Permissions.ToList();
        modifiablePermissions.Add(permission);
        Permissions = modifiablePermissions;

        AddDomainEvent(new RolePermissionAddedEvent(Id, permission));
    }

    public void RemovePermission(PermissionId permission)
    {
        if (!Has(permission)) return;

        var modifiablePermissions = Permissions.ToList();
        modifiablePermissions.Remove(permission);
        Permissions = modifiablePermissions;

        AddDomainEvent(new RolePermissionRemovedEvent(Id, permission));
    }

    public bool Has(PermissionId permission)
    {
        return Permissions.Any(id => id == permission);
    }
}