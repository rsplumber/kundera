using FluentValidation;
using Managements.Domain.Roles;
using Mediator;

namespace Managements.Application.Roles;

public sealed record CreateRoleCommand : ICommand<Role>
{
    public string Name { get; init; } = default!;

    public IDictionary<string, string>? Meta { get; init; }
}

internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Role>
{
    private readonly IRoleFactory _roleFactory;

    public CreateRoleCommandHandler(IRoleFactory roleFactory)
    {
        _roleFactory = roleFactory;
    }

    public async ValueTask<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleFactory.CreateAsync(command.Name, command.Meta);
        return role;
    }
}

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}