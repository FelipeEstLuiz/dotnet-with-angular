using System.Text.Json.Serialization;

namespace Application.Domain.Entities;

public class User : Entity
{
    public required string UserName { get; set; }
    public string NormalizedUserName { get; set; } = string.Empty;
    public required string Email { get; set; }
    public string NormalizedEmail { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public required string KnowAs { get; set; }
    public DateTime LastActive { get; set; }
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public List<Photo> Photos { get; set; } = [];

    [JsonIgnore]
    public List<UserLike> LikedUsers { get; set; } = [];

    [JsonIgnore]
    public List<UserLike> LikedByUsers { get; set; } = [];

    public static User Create(
        string name,
        string email,
        string knowAs,
        string gender,
        DateOnly dateOfBirth,
        string? introduction,
        string? interests,
        string? lookingFor,
        string? city,
        string? country
    ) => new()
    {
        KnowAs = knowAs,
        Gender = gender,
        Introduction = introduction,
        DateOfBirth = dateOfBirth,
        Email = email,
        UserName = name,
        NormalizedUserName = name.ToUpperInvariant(),
        NormalizedEmail = email.ToUpperInvariant(),
        LastActive = DateTime.UtcNow,
        Interests = interests,
        LookingFor = lookingFor,
        City = city,
        Country = country
    };

    public void SetPassword(string password) => PasswordHash = password;
    public void SetGender(string gender) => Gender = gender;
    public void SetKowAs(string knowAs) => KnowAs = knowAs;
    public void SetIntroduction(string? introduction) => Introduction = introduction;
    public void SetInterests(string? interests) => Interests = interests;
    public void SetLookingFor(string? lookingFor) => LookingFor = lookingFor;
    public void SetCity(string? city) => City = city;
    public void SetCountry(string? country) => Country = country;
    public void SetKnowAs(string knowAs) => KnowAs = knowAs;

    public void UpdateLastActive() => LastActive = DateTime.UtcNow;

    public void SetName(string name)
    {
        UserName = name;
        NormalizedUserName = name.ToUpperInvariant();
    }

    public void SetEmail(string email)
    {
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
    }

}
