using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Mediator;

namespace Application.Groups;

public sealed record DeleteGroupCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;
}

internal sealed class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DeleteGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }


    public async ValueTask<Unit> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteAsync(group.Id, cancellationToken);

        return Unit.Value;
    }
}