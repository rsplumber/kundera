using Core.Groups;
using Core.Roles;
using Core.Users.Events;
using Core.Users.Exception;

namespace Core.Users;

public class User : BaseEntity
{
    public User()
    {
    }

    internal User(string username, Group group)
    {
        AddUsername(username);

        Join(group);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public List<string> Usernames { get; set; } = new();

    public List<Group> Groups { get; set; } = new();

    public List<Role> Roles { get; set; } = new();

    public UserStatus Status { get; set; } = default!;

    public string? StatusChangeReason { get; set; }

    public DateTime StatusChangeDateUtc { get; set; }

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

    public void Join(Group group)
    {
        if (Has(group)) return;
        Groups.Add(group);
        AddDomainEvent(new UserJoinedGroupEvent(Id, group.Id));
    }

    public void Leave(Group group)
    {
        if (!Has(group)) return;
        if (Groups.Count == 1)
        {
            throw new UsernameGroupCouldNotBeEmptyException();
        }

        Groups.Remove(group);
        AddDomainEvent(new UserRemovedGroupEvent(Id, group.Id));
    }

    public bool Has(Group group) => Groups.Any(g => g == group);

    public void Assign(Role role)
    {
        if (Has(role)) return;
        Roles.Add(role);
        AddDomainEvent(new UserRoleAddedEvent(Id, role.Id));
    }

    public void Revoke(Role role)
    {
        if (!Has(role)) return;
        Roles.Remove(role);
        AddDomainEvent(new UserRoleRemovedEvent(Id, role.Id));
    }

    public bool Has(Role role) =>Roles.Any(r => r == role);

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