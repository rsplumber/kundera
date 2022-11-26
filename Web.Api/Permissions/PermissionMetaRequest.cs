using Application.Permissions;
using Core.Domains.Permissions.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Permissions;

public record AddPermissionMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public ChangePermissionMetaCommand ToCommand(Guid permissionId) => new(PermissionId.From(permissionId), Meta);
}

public class AddPermissionMetaRequestValidator : RequestValidator<AddPermissionMetaRequest>
{
    public AddPermissionMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty()
            .WithMessage("Enter valid Meta")
            .NotNull()
            .WithMessage("Enter valid Meta");
    }
}

public record RemovePermissionMetaRequest(string[] MetaKeys) : IWebRequest
{
    public RemovePermissionMetaCommand ToCommand(Guid permissionId) => new(PermissionId.From(permissionId), MetaKeys);
}

public class RemovePermissionMetaRequestValidator : RequestValidator<RemovePermissionMetaRequest>
{
    public RemovePermissionMetaRequestValidator()
    {
        RuleFor(request => request.MetaKeys)
            .NotEmpty()
            .WithMessage("Enter valid Meta")
            .NotNull()
            .WithMessage("Enter valid Meta");
    }
}