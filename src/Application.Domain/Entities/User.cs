using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Application.Domain.Entities;

public class User : IdentityUser
{
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateOnly DateOfBirth { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime LastActive { get; set; }
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    [JsonIgnore]
    public List<Photo> Photos { get; set; } = [];

    [JsonIgnore]
    public List<UserLike> LikedUsers { get; set; } = [];

    [JsonIgnore]
    public List<UserLike> LikedByUsers { get; set; } = [];

    [JsonIgnore]
    public List<Message> MessagesSent { get; set; } = [];

    [JsonIgnore]
    public List<Message> MessagesReceived { get; set; } = [];

    public static User Create(
        string fullName,
        string email,
        string gender,
        DateOnly dateOfBirth,
        string? introduction,
        string? interests,
        string? lookingFor,
        string city,
        string country
    )
    {
        User user = new()
        {
            Gender = gender,
            Introduction = introduction,
            DateOfBirth = dateOfBirth,
            LastActive = DateTime.UtcNow,
            Interests = interests,
            LookingFor = lookingFor,
            City = city,
            Country = country
        };

        user.SetName(fullName);
        user.SetEmail(email);

        return user;
    }

    public void SetPassword(string password) => PasswordHash = password;
    public void SetGender(string gender) => Gender = gender;
    public void SetIntroduction(string? introduction) => Introduction = introduction;
    public void SetInterests(string? interests) => Interests = interests;
    public void SetLookingFor(string? lookingFor) => LookingFor = lookingFor;
    public void SetCity(string city) => City = city;
    public void SetCountry(string country) => Country = country;

    public void UpdateLastActive() => LastActive = DateTime.UtcNow;

    public void SetName(string fullName)
    {
        string[] names = fullName.Split(" ");
        string name = names[0];

        FullName = fullName;
        UserName = name;
        NormalizedUserName = name.ToUpperInvariant();
    }

    public void SetEmail(string? email)
    {
        Email = email;
        NormalizedEmail = email?.ToUpperInvariant();
    }
}
