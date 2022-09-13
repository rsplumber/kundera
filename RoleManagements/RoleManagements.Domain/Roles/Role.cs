using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Events;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Roles;

public class Role : AggregateRoot<RoleId>
{
    private readonly List<PermissionId> _permissions;
    private readonly Dictionary<string, string> _meta;

    protected Role()
    {
    }

    private Role(RoleId id) : base(id)
    {
        _permissions = new List<PermissionId>();
        _meta = new Dictionary<string, string>();
        AddDomainEvent(new RoleCreatedEvent(id));
    }

    public static async Task<Role> CreateAsync(Name name, IRoleRepository repository)
    {
        var id = RoleId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        return new Role(id);
    }

    public IReadOnlyCollection<PermissionId> Permissions => _permissions.AsReadOnly();

    public void AddPermission(PermissionId permission)
    {
        if (HasPermission(permission)) return;
        _permissions.Add(permission);
    }

    public void RemovePermission(PermissionId permission)
    {
        if (!HasPermission(permission)) return;
        _permissions.Remove(permission);
    }

    public bool HasPermission(PermissionId permission)
    {
        return _permissions.Exists(id => id == permission);
    }

    public IReadOnlyDictionary<string, string> Meta => _meta;

    public void AddMeta(string key, string value)
    {
        _meta.TryAdd(key, value);
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