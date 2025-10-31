using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class BaseUserValidator : AbstractValidator<BaseUserModel>
{
    public BaseUserValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Obrigatorio")
            .Length(3, 100).WithMessage("Deve ter pelo entre 3 e 100 caracteres.");

        RuleFor(x => x.Gender)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 50).WithMessage("Deve ter pelo entre 3 e 50 caracteres.");

        RuleFor(x => x.KnowAs)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 100).WithMessage("Deve ter pelo entre 3 e 100 caracteres.");

        RuleFor(x => x.City)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 200).WithMessage("Deve ter pelo entre 3 e 200 caracteres.");

        RuleFor(x => x.Country)
          .NotEmpty().WithMessage("Obrigatorio")
          .Length(3, 50).WithMessage("Deve ter pelo entre 3 e 50 caracteres.");

        RuleFor(x => x.Interests)
          .MaximumLength(1000).WithMessage("Deve ter no maximo 2000 caracteres.");

        RuleFor(x => x.LookingFor)
          .MaximumLength(1000).WithMessage("Deve ter no maximo 2000 caracteres.");

        RuleFor(x => x.Introduction)
          .MaximumLength(2000).WithMessage("Deve ter no maximo 2000 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Obrigatorio")
            .EmailAddress().WithMessage("Invalido")
            .MaximumLength(150).WithMessage("Pode ter no maximo 150 caracteres.");
    }
}
