using FluentValidation;
using Todo.Contract.Users;

namespace Todo.Contract.Validations;

public class MUserValidator : AbstractValidator<UpdateUserDto>,IValidator
{
    public MUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Không được bỏ trống first Name");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("không được bỏ trống last Name");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Không được bỏ trống password")
            .When(x=>x.IsSetPassword == true);
    }

}

