using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;
using Managements.Domain.Roles.Types;
using Managements.Domain.Users.Events;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users;

public class User : AggregateRoot
{
    protected User()
    {
    }

    internal User(Username username, GroupId groupId)
    {
        AddUsername(username);

        JoinGroup(groupId);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public UserId Id { get; set; } = UserId.Generate();

    public IReadOnlyCollection<Username> Usernames { get; internal set; } = new List<Username>();

    public IReadOnlyCollection<GroupId> Groups { get; internal set; } = new List<GroupId>();

    public IReadOnlyCollection<RoleId> Roles { get; internal set; } = new List<RoleId>();

    public UserStatus Status { get; internal set; } = default!;

    public Text? StatusChangeReason { get; internal set; }

    public DateTime StatusChangeDate { get; internal set; }

    private void ChangeReason(Text? reason) => StatusChangeReason = reason;

    public void AddUsername(Username username)
    {
        if (Has(username))
        {
            throw new UserDuplicateIdentifierException(username);
        }

        var modifiableUsernames = Usernames.ToList();
        modifiableUsernames.Add(username);
        Usernames = modifiableUsernames;

        AddDomainEvent(new UserUsernameAddedEvent(Id, username));
    }

    public void RemoveUsername(Username username)
    {
        if (!Has(username)) return;

        if (Usernames.Count == 1)
        {
            throw new UsernameCouldNotBeEmptyException();
        }

        var modifiableUsernames = Usernames.ToList();
        modifiableUsernames.Remove(username);
        Usernames = modifiableUsernames;

        AddDomainEvent(new UserUsernameRemovedEvent(Id, username));
    }

    public bool Has(Username username)
    {
        return Usernames.Any(u => u == username);
    }

    public void JoinGroup(GroupId group)
    {
        if (Has(group)) return;

        var modifiableGroups = Groups.ToList();
        modifiableGroups.Add(group);
        Groups = modifiableGroups;
        AddDomainEvent(new UserJoinedGroupEvent(Id, group));
    }

    public void RemoveFromGroup(GroupId group)
    {
        if (!Has(group)) return;

        if (Groups.Count == 1)
        {
            throw new UsernameGroupCouldNotBeEmptyException();
        }

        var modifiableGroups = Groups.ToList();
        modifiableGroups.Remove(group);
        Groups = modifiableGroups;

        AddDomainEvent(new UserRemovedGroupEvent(Id, group));
    }

    public bool Has(GroupId group)
    {
        return Groups.Any(id => id == group);
    }

    public void AssignRole(RoleId role)
    {
        if (Has(role)) return;

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Add(role);
        Roles = modifiableRoles;

        AddDomainEvent(new UserRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Remove(role);
        Roles = modifiableRoles;

        AddDomainEvent(new UserRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return Roles.Any(id => id == role);
    }

    public void Activate() => ChangeStatus(UserStatus.Active);

    public void Suspend(Text? reason = null) => ChangeStatus(UserStatus.Suspend, reason);

    public void Block(Text reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        Status = status;
        StatusChangeDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}