using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record DeleteUserGroupCommand(UserGroupId UserGroupId) : Command;

internal sealed class DeleteUserGroupCommandHandler : CommandHandler<DeleteUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public DeleteUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(DeleteUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var userGroup = await _userGroupRepository.FindAsync(message.UserGroupId, cancellationToken);
        if (userGroup is null)
        {
            throw new UserGroupNotFoundException();
        }

        await _userGroupRepository.DeleteAsync(userGroup.Id, cancellationToken);
    }
}