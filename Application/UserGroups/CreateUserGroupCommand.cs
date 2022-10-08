using Domain;
using Domain.Roles;
using Domain.Roles.Exceptions;
using Domain.UserGroups;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name, RoleId Role) : Command;

internal sealed class CreateUserGroupCommandHandler : CommandHandler<CreateUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateUserGroupCommandHandler(IUserGroupRepository userGroupRepository, IRoleRepository roleRepository)
    {
        _userGroupRepository = userGroupRepository;
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(CreateUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var userGroup = UserGroup.From(name, role.Id);
        await _userGroupRepository.AddAsync(userGroup, cancellationToken);
    }
}