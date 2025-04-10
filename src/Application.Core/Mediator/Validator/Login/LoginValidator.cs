using Application.Core.Mediator.Command.Login;
using FluentValidation;

namespace Application.Core.Mediator.Validator.Login;

public class LoginValidator: AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatório")
            .EmailAddress().WithMessage("Inválido")
            .MaximumLength(150).WithMessage("Pode ter no máximo 150 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Obrigatório");
    }
}
