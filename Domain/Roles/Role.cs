using Domain.Permissions;
using Domain.Permissions.Exceptions;
using Domain.Roles.Events;
using Domain.Roles.Exceptions;
using Tes.Domain.Contracts;

namespace Domain.Roles;

public class Role : AggregateRoot<RoleId>
{
    private readonly List<PermissionId> _permissions = new();
    private readonly Dictionary<string, string> _meta = new();

    protected Role()
    {
    }

    private Role(RoleId id) : base(id)
    {
        AddDomainEvent(new RoleCreatedEvent(id));
    }

    public static async Task<Role> CreateAsync(Name name, IRoleRepository repository)
    {
        var id = RoleId.From(name.Value.ToLower());
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        return new Role(id);
    }

    public IReadOnlyCollection<PermissionId> Permissions => _permissions.AsReadOnly();

    public async Task AddPermissionAsync(PermissionId permission, IPermissionRepository permissionRepository)
    {
        var permissionExist = await permissionRepository.ExistsAsync(permission);
        if (!permissionExist)
        {
            throw new PermissionNotFoundException();
        }

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