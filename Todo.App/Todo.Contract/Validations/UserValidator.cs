using FluentValidation;
using Todo.Contract.Users;
using Todo.Core.Consts.Regulars;
using Todo.Core.Validations;
using Todo.Localziration;

namespace Todo.Contract.Validations;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    
    public UpdateUserValidator(Localizer L)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(L["Validator.IsRequired"])
            .Matches(InputRegularExpression.Name)
            .WithMessage(L["Validator.InValidFormat"]);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(L["Validator.IsRequired"])
            .Matches(InputRegularExpression.Name);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(L["Validator.IsRequired"])
            .When(x=>x.IsSetPassword == true);
        
        RuleFor(x => x.Email)
            .Matches(InputRegularExpression.Email)
            .WithMessage(L["Validator.InValidFormat"]);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(InputRegularExpression.NumberPhone)
            .WithMessage(L["Validator.InValidFormat"]);
    }

}

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator(Localizer L)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(L["Validator.IsRequired"])
            .Matches(InputRegularExpression.Name)
            .WithMessage(L["Validator.InValidFormat"]);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(L["Validator.IsRequired"])
            .Matches(InputRegularExpression.Name);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(L["Validator.IsRequired"]);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(InputRegularExpression.NumberPhone)
            .WithMessage(L["Validator.InValidFormat"]);
        RuleFor(x => x.Email)
            .Matches(InputRegularExpression.Email)
            .WithMessage(L["Validator.InValidFormat"]);
    }
}
