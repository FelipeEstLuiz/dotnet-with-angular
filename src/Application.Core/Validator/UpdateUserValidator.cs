using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.City)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 200).WithMessage("Deve ter pelo entre 3 e 200 caracteres.");

        RuleFor(x => x.Country)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 50).WithMessage("Deve ter pelo entre 3 e 50 caracteres.");

        RuleFor(x => x.Interests)
          .MaximumLength(1000).WithMessage("Deve ter no maximo 2000 caracteres.");

        RuleFor(x => x.Introduction)
          .MaximumLength(2000).WithMessage("Deve ter no maximo 2000 caracteres.");
    }
}
