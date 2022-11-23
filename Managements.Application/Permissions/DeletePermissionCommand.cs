using FluentValidation;
using Managements.Domain.Permissions;
using Managements.Domain.Permissions.Exceptions;
using Managements.Domain.Permissions.Types;
using Mediator;

namespace Managements.Application.Permissions;

public sealed record DeletePermissionCommand : ICommand
{
    public Guid Permission { get; init; } = default;
}

internal sealed class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;

    public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(DeletePermissionCommand command, CancellationToken cancellationToken)
    {
        var permissionId = PermissionId.From(command.Permission);
        var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }

        await _permissionRepository.DeleteAsync(permissionId, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(request => request.Permission)
            .NotEmpty().WithMessage("Enter a Permission")
            .NotNull().WithMessage("Enter a Permission");
    }
}