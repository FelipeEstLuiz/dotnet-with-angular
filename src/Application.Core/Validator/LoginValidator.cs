using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class LoginValidator : AbstractValidator<LoginModel>
{
    public LoginValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Required")
            .EmailAddress().WithMessage("Invalid")
            .MaximumLength(150).WithMessage("It can have a maximum of 150 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Required");
    }
}
