namespace Application.Core.Model.User;

public record InsertUserModel : BaseUserModel
{
    public DateOnly? DateOfBirth { get; set; }
    public string Password { get; set; } = null!;
    public string PasswordConfirmed { get; set; } = null!;

    public Domain.Entities.User MapUsuario() => Domain.Entities.User.Create(
        fullName: FullName,
        email: Email,
        gender: Gender,
        dateOfBirth: DateOfBirth!.Value,
        introduction: Introduction,
        interests: Interests,
        lookingFor: LookingFor,
        city: City,
        country: Country
    );
}
