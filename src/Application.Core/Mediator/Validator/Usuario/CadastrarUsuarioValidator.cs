using Application.Core.Mediator.Command.Usuario;
using FluentValidation;

namespace Application.Core.Mediator.Validator.Usuario;

public class CadastrarUsuarioValidator : AbstractValidator<CadastrarUsuarioCommand>
{
    public CadastrarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatório")
            .MinimumLength(3).WithMessage("Deve ter pelo menos 3 caracteres.")
            .MaximumLength(100).WithMessage("Pode ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatório")
            .EmailAddress().WithMessage("Inválido")
            .MaximumLength(150).WithMessage("Pode ter no máximo 150 caracteres.");

        RuleFor(x => x.Senha)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatório")
            .MinimumLength(8).WithMessage("Deve ter pelo menos 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("Deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("Deve conter pelo menos uma letra minúscula.")
            .Matches(@"\d").WithMessage("Deve conter pelo menos um número.")
            .Matches(@"[@#$%^&+=!]").WithMessage("Deve conter pelo menos um caractere especial (@#$%^&+=!).");

        RuleFor(x => x.SenhaConfirmacao)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatório")
            .Equal(u => u.Senha).WithMessage("A confirmação de senha não corresponde à senha.");
    }
}
