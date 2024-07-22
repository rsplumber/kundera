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

    internal User(Group group)
    {
        Join(group);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }

    internal User(Guid id, Group group) : this(group)
    {
        Id = id;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public List<Group> Groups { get; set; } = new();

    public List<Role> Roles { get; set; } = new();

    public UserStatus Status { get; set; }

    public string? StatusChangeReason { get; set; }

    public DateTime StatusChangeDateUtc { get; set; }

    private void ChangeReason(string? reason) => StatusChangeReason = reason;


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

    public bool Has(Role role) => Roles.Any(r => r == role);

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