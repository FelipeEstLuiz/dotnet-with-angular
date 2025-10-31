using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class InsertUserValidator : AbstractValidator<InsertUserModel>
{
    public InsertUserValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatorio")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Invalida");

        RuleFor(x => x).SetValidator(new BaseUserValidator());

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Obrigatorio")
           .MinimumLength(8).WithMessage("Deve ter pelo menos 8 caracteres.")
           .Matches(@"[A-Z]").WithMessage("Deve conter pelo menos uma letra maiuscula.")
           .Matches(@"[a-z]").WithMessage("Deve conter pelo menos uma letra minuscula.")
           .Matches(@"\d").WithMessage("Deve conter pelo menos um numero.")
           .Matches(@"[@#$%^&+=!]").WithMessage("Deve conter pelo menos um caractere especial (@#$%^&+=!).");

        RuleFor(x => x.PasswordConfirmed)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Obrigatorio")
            .Equal(u => u.Password).WithMessage("A confirmacao de senha nao corresponde a senha.");
    }
}
