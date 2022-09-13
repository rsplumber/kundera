using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.UserRoles.Events;
using RoleManagements.Domain.UserRoles.Exceptions;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.UserRoles;

public class UserRole : AggregateRoot<UserId>
{
    private readonly List<RoleId> _roles;

    protected UserRole()
    {
    }

    private UserRole(UserId userId) : base(userId)
    {
        _roles = new List<RoleId>();
        AddDomainEvent(new UserRoleCreatedEvent(userId));
    }

    public static async Task<UserRole> CreateAsync(UserId userId, IUserRoleRepository repository)
    {
        var exists = await repository.ExistsAsync(userId);
        if (exists)
        {
            throw new UserRoleAlreadyExistsException(userId);
        }

        return new UserRole(userId);
    }


    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    public void AddRole(RoleId role)
    {
        if (HasRole(role)) return;
        _roles.Add(role);
    }

    public void RemoveRole(RoleId role)
    {
        if (!HasRole(role)) return;
        _roles.Remove(role);
    }

    public bool HasRole(RoleId role)
    {
        return _roles.Exists(id => id == role);
    }
}