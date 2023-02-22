using Core.Domains.Roles.Events;

namespace Core.Domains.Roles;

public class Role : BaseEntity
{
    protected Role()
    {
    }

    internal Role(string name, IDictionary<string, string>? meta = null)
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

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public HashSet<Guid> Permissions { get; internal set; } = new();

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(string name) => Name = name;

    public void AddPermission(Guid permission)
    {
        if (HasPermission(permission)) return;
        Permissions.Add(permission);
        AddDomainEvent(new RolePermissionAddedEvent(Id, permission));
    }

    public void RemovePermission(Guid permission)
    {
        if (!HasPermission(permission)) return;
        Permissions.Remove(permission);
        AddDomainEvent(new RolePermissionRemovedEvent(Id, permission));
    }

    public bool HasPermission(Guid permission)
    {
        return Permissions.Any(id => id == permission);
    }
}