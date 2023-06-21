using FluentValidation;
using Todo.Contract.Roles;
using Todo.Localziration;

namespace Todo.Contract.Validations;

public class RoleValidator : AbstractValidator<CreateUpdateRoleDto>
{
    public RoleValidator(Localizer L)
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage(L["Validator.IsRequired"]);
        RuleFor(x => x.Name).NotEmpty().WithMessage(L["Validator.IsRequired"]);
    }

}