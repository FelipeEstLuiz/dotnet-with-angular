using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class InsertUserValidator : AbstractValidator<InsertUserModel>
{
    public InsertUserValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Required")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Invalid");

        RuleFor(x => x).SetValidator(new BaseUserValidator());

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Required")
           .MinimumLength(8).WithMessage("It must have at least 8 characters.")
           .Matches(@"[A-Z]").WithMessage("It must have at least one capital letter.")
           .Matches(@"[a-z]").WithMessage("It must have at least one lowercase letter.")
           .Matches(@"\d").WithMessage("It must have at least one number.")
           .Matches(@"[@#$%^&+=!]").WithMessage("It must have at least one special character (@#$%^&+=!).");

        RuleFor(x => x.PasswordConfirmed)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Required")
            .Equal(u => u.Password).WithMessage("The password confirmation does not match the password.");
    }
}
