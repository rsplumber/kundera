using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Events;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Roles;

public class Role : AggregateRoot<RoleId>
{
    private readonly string _title;
    private readonly List<PermissionId> _permissions;

    protected Role()
    {
    }

    private Role(RoleId id, Name title) : base(id)
    {
        _permissions = new List<PermissionId>();
        _title = title;
        AddDomainEvent(new RoleCreatedEvent(id));
    }

    public static async Task<Role> CreateAsync(Name name, Name title, IRoleRepository repository)
    {
        var id = RoleId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new RoleAlreadyExistsException(name);
        }

        return new Role(id, title);
    }

    public string Title => _title;

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
}