using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class PhotoUploadModelValidator : AbstractValidator<PhotoUploadModel>
{
    public PhotoUploadModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.File)
            .NotNull().WithMessage("O arquivo é obrigatório.")
            .Must(f => f.Length > 0).WithMessage("O arquivo não pode estar vazio.")
            .Must(f => f.ContentType.StartsWith("image/"))
            .WithMessage("O arquivo deve ser uma imagem.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("O nome de usuário é obrigatório.")
            .MaximumLength(100);
    }
}
