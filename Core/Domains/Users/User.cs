using Core.Domains.Users.Events;
using Core.Domains.Users.Exception;

namespace Core.Domains.Users;

public class User : BaseEntity
{
    protected User()
    {
    }

    internal User(string username, Guid groupId)
    {
        AddUsername(username);

        JoinGroup(groupId);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public HashSet<string> Usernames { get; internal set; } = new();

    public HashSet<Guid> Groups { get; internal set; } = new();

    public HashSet<Guid> Roles { get; internal set; } = new();

    public UserStatus Status { get; internal set; } = default!;

    public string? StatusChangeReason { get; internal set; }

    public DateTime StatusChangeDateUtc { get; internal set; }

    private void ChangeReason(string? reason) => StatusChangeReason = reason;

    public void AddUsername(string username)
    {
        if (HasUsername(username))
        {
            throw new UserDuplicateIdentifierException(username);
        }

        Usernames.Add(username);
        AddDomainEvent(new UserUsernameAddedEvent(Id, username));
    }

    public void RemoveUsername(string username)
    {
        if (!HasUsername(username)) return;

        if (Usernames.Count == 1)
        {
            throw new UsernameCouldNotBeEmptyException();
        }

        Usernames.Remove(username);
        AddDomainEvent(new UserUsernameRemovedEvent(Id, username));
    }

    public bool HasUsername(string username)
    {
        return Usernames.Any(u => u == username);
    }

    public void JoinGroup(Guid group)
    {
        if (HasGroup(group)) return;
        Groups.Add(group);
        AddDomainEvent(new UserJoinedGroupEvent(Id, group));
    }

    public void RemoveFromGroup(Guid group)
    {
        if (!HasGroup(group)) return;
        if (Groups.Count == 1)
        {
            throw new UsernameGroupCouldNotBeEmptyException();
        }

        Groups.Remove(group);
        AddDomainEvent(new UserRemovedGroupEvent(Id, group));
    }

    public bool HasGroup(Guid group)
    {
        return Groups.Any(id => id == group);
    }

    public void AssignRole(Guid role)
    {
        if (HasRole(role)) return;
        Roles.Add(role);
        AddDomainEvent(new UserRoleAddedEvent(Id, role));
    }

    public void RevokeRole(Guid role)
    {
        if (!HasRole(role)) return;
        Roles.Remove(role);
        AddDomainEvent(new UserRoleRemovedEvent(Id, role));
    }

    public bool HasRole(Guid role)
    {
        return Roles.Any(id => id == role);
    }

    public void Activate() => ChangeStatus(UserStatus.Active);

    public void Suspend(string? reason = null) => ChangeStatus(UserStatus.Suspend, reason);

    public void Block(string reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, string? reason = null)
    {
        Status = status;
        StatusChangeDateUtc = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}