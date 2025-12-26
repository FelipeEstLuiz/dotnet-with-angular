using Application.Core.Model.User;
using FluentValidation;

namespace Application.Core.Validator;

public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Required")
           .Length(3, 100).WithMessage("Must be between 3 and 100 characters.");

        RuleFor(x => x.City)
             .NotEmpty().WithMessage("Required")
             .Length(2, 200).WithMessage("Must be between 3 and 200 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Required")
            .Length(2, 50).WithMessage("Must be between 3 and 50 characters.");

        RuleFor(x => x.Interests)
            .MaximumLength(1000).WithMessage("Can have at most 2000 characters.");

        RuleFor(x => x.LookingFor)
            .MaximumLength(1000).WithMessage("Can have at most 2000 characters.");

        RuleFor(x => x.Introduction)
            .MaximumLength(2000).WithMessage("Can have at most 2000 characters.");
    }
}
