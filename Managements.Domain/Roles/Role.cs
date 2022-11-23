using Managements.Domain.Contracts;
using Managements.Domain.Permissions.Types;
using Managements.Domain.Roles.Events;
using Managements.Domain.Roles.Types;

namespace Managements.Domain.Roles;

public class Role : AggregateRoot
{
    protected Role()
    {
    }

    internal Role(Name name, IDictionary<string, string>? meta = null)
    {
        Name = name;
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                Meta.Add(key, value);
            }
        }

        AddDomainEvent(new RoleCreatedEvent(Id));
    }

    public RoleId Id { get; set; } = RoleId.Generate();

    public Name Name { get; internal set; } = default!;

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