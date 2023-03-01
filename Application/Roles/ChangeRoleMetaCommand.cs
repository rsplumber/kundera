using System.Collections.Immutable;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;

namespace Application.Roles;

public sealed record ChangeRoleMetaCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public IDictionary<string, string> Meta { get; init; } = ImmutableDictionary<string, string>.Empty;
};

internal sealed class ChangeRoleMetaCommandHandler : ICommandHandler<ChangeRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public ChangeRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(ChangeRoleMetaCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        role.Meta.Clear();
        foreach (var (key, value) in command.Meta)
        {
            role.Meta.Add(key, value);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}