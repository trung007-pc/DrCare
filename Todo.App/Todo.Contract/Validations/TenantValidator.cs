using FluentValidation;
using Todo.Contract.Tenants;
using Todo.Core.Consts.Regulars;
using Todo.Localziration;

namespace Todo.Contract.Validations;

public class TenantValidator : AbstractValidator<CreateUpdateTenantDto>
{ 
    public TenantValidator(Localizer L)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(L["Validator.IsRequired"]);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(L["Validator.IsRequired"])
            .Matches(InputRegularExpression.NumberPhone)
            .WithMessage(L["Validator.InValidFormat"]);
    }
}