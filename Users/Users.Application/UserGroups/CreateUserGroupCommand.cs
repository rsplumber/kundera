using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name, RoleId Role) : Command;

internal sealed class CreateUserGroupCommandHandler : CommandHandler<CreateUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public CreateUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(CreateUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var userGroup = UserGroup.Create(name, roleId);
        await _userGroupRepository.AddAsync(userGroup, cancellationToken);
    }
}