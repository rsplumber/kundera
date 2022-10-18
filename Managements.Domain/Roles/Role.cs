using Kite.Domain.Contracts;
using Managements.Domain.Permissions;
using Managements.Domain.Roles.Events;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Domain.Roles;

public class Role : AggregateRoot<RoleId>
{
    private string _name;
    private readonly List<PermissionId> _permissions = new();
    private readonly Dictionary<string, string> _meta = new();

    protected Role()
    {
    }

    private Role(Name name) : base(RoleId.Generate())
    {
        _name = name;
        AddDomainEvent(new RoleCreatedEvent(Id));
    }

    public static async Task<Role> FromAsync(Name name, IRoleRepository repository)
    {
        var exists = await repository.ExistsAsync(name);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        return new Role(name);
    }

    public string Name => _name;

    public IReadOnlyCollection<PermissionId> Permissions => _permissions;

    public IReadOnlyDictionary<string, string> Meta => _meta;

    public void ChangeName(Name name) => _name = name;

    public void AddPermission(PermissionId permission)
    {
        if (Has(permission)) return;

        _permissions.Add(permission);
        AddDomainEvent(new RolePermissionAddedEvent(Id, permission));
    }

    public void RemovePermission(PermissionId permission)
    {
        if (!Has(permission)) return;

        _permissions.Remove(permission);
        AddDomainEvent(new RolePermissionRemovedEvent(Id, permission));
    }

    public bool Has(PermissionId permission)
    {
        return _permissions.Any(id => id == permission);
    }

    public void AddMeta(string key, string value)
    {
        RemoveMeta(key);
        _meta.Add(key, value);
    }

    public void RemoveMeta(string key)
    {
        if (GetMetaValue(key) is null) return;

        _meta.Remove(key);
    }

    public string? GetMetaValue(string key)
    {
        _meta.TryGetValue(key, out var value);

        return value;
    }
}