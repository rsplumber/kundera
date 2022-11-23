using System.Collections.Immutable;
using FluentValidation;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Roles.Types;
using Mediator;

namespace Managements.Application.Roles;

public sealed record ChangeRoleMetaCommand : ICommand
{
    public Guid Role { get; init; } = default!;

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
        var role = await _roleRepository.FindAsync(RoleId.From(command.Role), cancellationToken);
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
        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter a Role")
            .NotNull().WithMessage("Enter a Role");
    }
}