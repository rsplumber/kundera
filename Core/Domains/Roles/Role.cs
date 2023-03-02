using Core.Domains.Permissions;
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
        FillMeta(meta);
        AddDomainEvent(new RoleCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public HashSet<Permission> Permissions { get; internal set; } = new();

    public IDictionary<string, string> Meta { get; internal set; } = new Dictionary<string, string>();

    public void ChangeName(string name) => Name = name;

    public void Add(Permission permission)
    {
        if (Has(permission)) return;
        Permissions.Add(permission);
        AddDomainEvent(new RolePermissionAddedEvent(Id, permission.Id));
    }

    public void Remove(Permission permission)
    {
        if (!Has(permission)) return;
        Permissions.Remove(permission);
        AddDomainEvent(new RolePermissionRemovedEvent(Id, permission.Id));
    }

    public bool Has(Permission permission) => Permissions.Any(p => p == permission);
    
    private void FillMeta(IDictionary<string, string>? meta)
    {
        if (meta is null) return;
        foreach (var (key, value) in meta)
        {
            Meta.TryAdd(key, value);
        }
    }
}