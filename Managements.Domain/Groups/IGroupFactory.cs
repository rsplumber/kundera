using Managements.Domain.Groups.Exception;
using Managements.Domain.Roles;

namespace Managements.Domain.Groups;

public interface IGroupFactory
{
    Task<Group> CreateAsync(Name name, RoleId role);

    Task<Group> CreateAsync(Name name, RoleId role, GroupId parent);
}

internal sealed class GroupFactory : IGroupFactory
{
    private readonly IGroupRepository _groupRepository;


    public GroupFactory(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group> CreateAsync(Name name, RoleId role)
    {
        var group = await _groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        return new Group(name, role);
    }

    public async Task<Group> CreateAsync(Name name, RoleId role, GroupId parent)
    {
        var group = await _groupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new GroupNameDuplicateException();
        }

        return new Group(name, role, parent);
    }
}