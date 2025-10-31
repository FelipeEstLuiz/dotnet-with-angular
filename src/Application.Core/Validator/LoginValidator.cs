using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class LoginValidator : AbstractValidator<LoginModel>
{
    public LoginValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Obrigatorio")
            .EmailAddress().WithMessage("Invalido")
            .MaximumLength(150).WithMessage("Pode ter no maximo 150 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Obrigatorio");
    }
}
