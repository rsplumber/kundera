using System.Collections.Immutable;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Roles.Types;
using FluentValidation;
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
        var role = await _roleRepository.FindAsync(RoleId.From(command.RoleId), cancellationToken);
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

public sealed class ChangeRoleMetaCommandValidator : AbstractValidator<ChangeRoleMetaCommand>
{
    public ChangeRoleMetaCommandValidator()
    {
        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter a Role")
            .NotNull().WithMessage("Enter a Role");
    }
}