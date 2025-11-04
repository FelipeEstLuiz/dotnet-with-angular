namespace Application.Domain.Entities;

public class User : Entity
{
    public required string UserName { get; set; }
    public string NormalizedUserName { get; set; } = string.Empty;
    public required string Email { get; set; }
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string SecurityStamp { get; set; } = string.Empty;
    public string ConcurrencyStamp { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
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
    public List<Photo> Photos { get; set; } = [];

    public static User Create(
        string name,
        string email,
        string knowAs,
        string gender,
        string? introduction,
        DateOnly dateOfBirth
    ) => new()
    {
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
        KnowAs = knowAs,
        Gender = gender,
        Introduction = introduction,
        DateOfBirth = dateOfBirth,
        Email = email,
        UserName = name,
        NormalizedUserName = name.ToUpperInvariant(),
        NormalizedEmail = email.ToUpperInvariant(),
        LastActive = DateTime.UtcNow
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
