using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class LoginValidator: AbstractValidator<LoginModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatorio")
            .EmailAddress().WithMessage("Invalido")
            .MaximumLength(150).WithMessage("Pode ter no maximo 150 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Obrigatorio");
    }
}
