using Kite.Domain.Contracts;
using Managements.Domain.Groups.Events;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Managements.Domain.Roles;

namespace Managements.Domain.Groups;

public class Group : AggregateRoot<GroupId>
{
    private string _name;
    private string? _description;
    private GroupId? _parent;
    private GroupStatus _status;
    private DateTime _statusChangedDate;
    private readonly List<RoleId> _roles = new();

    protected Group()
    {
    }

    private Group(string name, RoleId role) : base(GroupId.Generate())
    {
        _name = name;

        _roles = new List<RoleId>();
        AssignRole(role);

        ChangeStatus(GroupStatus.Enable);
        AddDomainEvent(new GroupCreatedEvent(Id));
    }

    private Group(string name, RoleId role, GroupId parent) : this(name, role)
    {
        _parent = parent;
    }

    public static async Task<Group> FromAsync(Name name, RoleId role, IGroupRepository groupRepository)
    {
        var group = await groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        return new Group(name, role);
    }

    public static async Task<Group> FromAsync(Name name, RoleId role, GroupId parent, IGroupRepository groupRepository)
    {
        var group = await groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        return new Group(name, role, parent);
    }

    public string Name => _name;

    public string? Description => _description;

    public GroupId? Parent => _parent;

    public GroupStatus GroupStatus => _status;

    public DateTime? StatusChangedDate => _statusChangedDate;

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    public void ChangeName(Name name)
    {
        _name = name;
    }

    public void ChangeDescription(Text description)
    {
        _description = description;
    }

    public void SetParent(GroupId parent)
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, parent, Parent));
        _parent = parent;
    }

    public void RemoveParent()
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, null, Parent));
        _parent = null;
    }


    public void AssignRole(RoleId role)
    {
        if (Has(role)) return;

        _roles.Add(role);
        AddDomainEvent(new GroupRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;

        if (Roles.Count == 1)
        {
            throw new GroupRoleCouldNotBeEmptyException();
        }

        _roles.Remove(role);
        AddDomainEvent(new GroupRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return _roles.Any(id => id == role);
    }

    public bool HasParent() => _parent is not null;

    public async Task<IEnumerable<Group>> ParentsAsync(IGroupRepository groupRepository)
    {
        var groups = new List<Group>();
        groups.Add(this);
        await FetchParentsAsync(this);
        return groups;

        async Task FetchParentsAsync(Group @group)
        {
            while (true)
            {
                if (@group.HasParent())
                {
                    var org = await groupRepository.FindAsync(@group.Parent!);
                    if (org is null) continue;
                    @group = org;
                    groups.Add(org);
                    continue;
                }

                break;
            }
        }
    }

    public async Task<IEnumerable<Role>> AllWithParentRolesAsync(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        var groups = await ParentsAsync(groupRepository);

        var roleIds = groups.SelectMany(group => group.Roles.Select(id => id)).ToArray();

        return await roleRepository.FindAsync(roleIds);
    }

    public void Enable() => ChangeStatus(GroupStatus.Enable);

    public void Disable() => ChangeStatus(GroupStatus.Disable);

    private void ChangeStatus(GroupStatus status)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        AddDomainEvent(new GroupStatusChangedEvent(Id, status));
    }
}