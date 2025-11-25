using Application.Core.Model.Admin;
using FluentValidation;

namespace Application.Core.Validator;

public class EditUserRolesValidator : AbstractValidator<EditUserRolesModel>
{
    public EditUserRolesValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("Required");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("Required");
    }
}
