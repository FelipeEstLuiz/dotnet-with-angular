using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class CadastrarUsuarioValidator : AbstractValidator<CadastrarUsuarioModel>
{
    public CadastrarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Obrigatorio")
            .Length(3, 100).WithMessage("Deve ter pelo entre 3 e 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Obrigatorio")
            .EmailAddress().WithMessage("Invalido")
            .MaximumLength(150).WithMessage("Pode ter no maximo 150 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Obrigatorio")
            .MinimumLength(8).WithMessage("Deve ter pelo menos 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("Deve conter pelo menos uma letra maiuscula.")
            .Matches(@"[a-z]").WithMessage("Deve conter pelo menos uma letra minuscula.")
            .Matches(@"\d").WithMessage("Deve conter pelo menos um numero.")
            .Matches(@"[@#$%^&+=!]").WithMessage("Deve conter pelo menos um caractere especial (@#$%^&+=!).");

        RuleFor(x => x.SenhaConfirmacao)
            .NotEmpty().WithMessage("Obrigatorio")
            .Equal(u => u.Senha).WithMessage("A confirmacao de senha nao corresponde a senha.");
    }
}
