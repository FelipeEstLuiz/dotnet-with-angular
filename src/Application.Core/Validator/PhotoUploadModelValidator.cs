using Application.Core.Model;
using FluentValidation;

namespace Application.Core.Validator;

public class PhotoUploadModelValidator : AbstractValidator<PhotoUploadModel>
{
    public PhotoUploadModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.File)
            .NotNull().WithMessage("Required.")
            .Must(f => f.Length > 0).WithMessage("The file must be empty.")
            .Must(f => f.ContentType.StartsWith("image/"))
            .WithMessage("The file must be an image..");
    }
}
