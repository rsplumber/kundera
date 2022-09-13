using RoleManagements.Domain.Permissions.Events;
using RoleManagements.Domain.Permissions.Exceptions;
using RoleManagements.Domain.Permissions.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Permissions;

public class Permission : AggregateRoot<PermissionId>
{
    private readonly string _title;

    protected Permission()
    {
    }

    private Permission(PermissionId id, Name title) : base(id)
    {
        _title = title;
        AddDomainEvent(new PermissionCreatedEvent(id));
    }

    public static async Task<Permission> CreateAsync(Name name, Name title, IPermissionRepository repository)
    {
        var id = PermissionId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new PermissionAlreadyExistsException(name);
        }

        return new Permission(id, title);
    }
}