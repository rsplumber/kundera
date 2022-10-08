using Application.Permissions;
using Domain.Permissions;
using FluentValidation;
using Tes.Web.Validators;

namespace Web.Api.Permissions;

public record AddPermissionMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public AddPermissionMetaCommand ToCommand(string permissionId) => new(PermissionId.From(permissionId), Meta);
}

public class AddPermissionMetaRequestValidator : RequestValidator<AddPermissionMetaRequest>
{
    public AddPermissionMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}

public record RemovePermissionMetaRequest(string[] MetaKeys) : IWebRequest
{
    public RemovePermissionMetaCommand ToCommand(string permissionId) => new(PermissionId.From(permissionId), MetaKeys);
}

public class RemovePermissionMetaRequestValidator : RequestValidator<RemovePermissionMetaRequest>
{
    public RemovePermissionMetaRequestValidator()
    {
        RuleFor(request => request.MetaKeys)
            .NotEmpty().WithMessage("Enter valid Meta")
            .NotNull().WithMessage("Enter valid Meta");
    }
}