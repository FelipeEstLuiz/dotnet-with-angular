using Application.Domain.Entities;

namespace Application.Core.Model;

public record InsertUserModel : BaseUserModel
{
    public DateOnly? DateOfBirth { get; set; }
    public string Password { get; set; } = null!;
    public string PasswordConfirmed { get; set; } = null!;

    public User MapUsuario() => User.Create(
        name: Name,
        email: Email,
        knowAs: KnowAs,
        gender: Gender,
        dateOfBirth: DateOfBirth!.Value,
        introduction: Introduction,
        interests: Interests,
        lookingFor: LookingFor,
        city: City,
        country: Country
    );
}
