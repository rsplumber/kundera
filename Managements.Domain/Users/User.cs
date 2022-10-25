using Kite.Domain.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Roles;
using Managements.Domain.Users.Events;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users;

public class User : AggregateRoot<UserId>
{
    private readonly List<Username> _usernames = new();
    private readonly List<GroupId> _groups = new();
    private readonly List<RoleId> _roles = new();
    private UserStatus _status;
    private string? _statusChangedReason;
    private DateTime _statusChangedDate;

    protected User()
    {
    }

    private User(Username username, GroupId groupId) : base(UserId.Generate())
    {
        AddUsername(username);

        JoinGroup(groupId);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }


    public static async Task<User> CreateAsync(Username username, GroupId groupId, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(username);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(username);
        }

        return new User(username, groupId);
    }


    public IReadOnlyCollection<Username> Usernames => _usernames.AsReadOnly();

    public string? Reason => _statusChangedReason;

    public UserStatus Status => _status;

    public DateTime StatusChangedDate => _statusChangedDate;

    public IReadOnlyCollection<GroupId> Groups => _groups.AsReadOnly();

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    private void ChangeReason(Text? reason) => _statusChangedReason = reason ?? null;

    public void AddUsername(Username username)
    {
        if (Has(username))
        {
            throw new UserDuplicateIdentifierException(username);
        }

        _usernames.Add(username);

        AddDomainEvent(new UserUsernameAddedEvent(Id, username));
    }

    public void RemoveUsername(Username username)
    {
        if (!Has(username)) return;

        if (Usernames.Count == 1)
        {
            throw new UsernameCouldNotBeEmptyException();
        }

        _usernames.Remove(username);
        AddDomainEvent(new UserUsernameRemovedEvent(Id, username));
    }

    public bool Has(Username username)
    {
        return _usernames.Any(u => u == username);
    }

    public void JoinGroup(GroupId group)
    {
        if (Has(group)) return;

        _groups.Add(group);
        AddDomainEvent(new UserJoinedGroupEvent(Id, group));
    }

    public void RemoveFromGroup(GroupId group)
    {
        if (!Has(group)) return;

        if (Groups.Count == 1)
        {
            throw new UsernameGroupCouldNotBeEmptyException();
        }

        _groups.Remove(group);
        AddDomainEvent(new UserRemovedGroupEvent(Id, group));
    }

    public async Task<IEnumerable<Group>> ParentsAsync(IGroupRepository groupRepository)
    {
        var groups = await groupRepository.FindAsync(Groups);
        var parentGroups = new List<Group>();
        foreach (var group in groups)
        {
            parentGroups.AddRange(await group.ParentsAsync(groupRepository));
        }

        return parentGroups;
    }

    public async Task<IEnumerable<Role>> RolesWithParentRolesAsync(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        var roles = new List<Role>();
        roles.AddRange(await roleRepository.FindAsync(Roles));
        var groups = await ParentsAsync(groupRepository);
        foreach (var group in groups)
        {
            roles.AddRange(await group.AllWithParentRolesAsync(groupRepository, roleRepository));
        }

        return roles;
    }

    public bool Has(GroupId group)
    {
        return _groups.Any(id => id == group);
    }

    public void AssignRole(RoleId role)
    {
        if (Has(role)) return;

        _roles.Add(role);
        AddDomainEvent(new UserRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;

        _roles.Remove(role);
        AddDomainEvent(new UserRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return _roles.Any(id => id == role);
    }

    public void Activate() => ChangeStatus(UserStatus.Active, null);

    public void Suspend(Text? reason = null) => ChangeStatus(UserStatus.Suspend, reason);

    public void Block(Text reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}