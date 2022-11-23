using System.Collections.Immutable;
using FluentValidation;
using Managements.Domain.Permissions;
using Managements.Domain.Permissions.Exceptions;
using Managements.Domain.Permissions.Types;
using Mediator;

namespace Managements.Application.Permissions;

public sealed record ChangePermissionMetaCommand : ICommand
{
    public Guid Permission { get; init; } = default!;

    public IDictionary<string, string> Meta { get; init; } = ImmutableDictionary<string, string>.Empty;
}

internal sealed class ChangePermissionMetaCommandHandler : ICommandHandler<ChangePermissionMetaCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public ChangePermissionMetaCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(ChangePermissionMetaCommand command, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.FindAsync(PermissionId.From(command.Permission), cancellationToken);

        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        permission.Meta.Clear();
        foreach (var (key, value) in command.Meta)
        {
            permission.Meta.Add(key, value);
        }

        await _permissionRepository.UpdateAsync(permission, cancellationToken);

        return Unit.Value;
    }
}

public sealed class ChangePermissionMetaCommandValidator : AbstractValidator<ChangePermissionMetaCommand>
{
    public ChangePermissionMetaCommandValidator()
    {
        RuleFor(request => request.Permission)
            .NotEmpty().WithMessage("Enter a Permission")
            .NotNull().WithMessage("Enter a Permission");
    }
}