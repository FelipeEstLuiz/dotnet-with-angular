using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserValidator() => RuleFor(x => x).SetValidator(new BaseUserValidator());
}
