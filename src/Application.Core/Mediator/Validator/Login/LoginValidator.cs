using Application.Core.Mediator.Command.Login;
using FluentValidation;

namespace Application.Core.Mediator.Validator.Login;

public class LoginValidator: AbstractValidator<LoginCommand>
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
