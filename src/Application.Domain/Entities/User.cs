using Application.Domain.Extensions;

namespace Application.Domain.Entities;

public class User : Entity
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string NormalizedUserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
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
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
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
        Id = Guid.NewGuid(),
        Email = email,
        NormalizedEmail = email.ToUpperInvariant(),
        UserName = name,
        NormalizedUserName = name.ToUpperInvariant(),
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
        KnowAs = knowAs,
        Gender = gender,
        Introduction = introduction,
        DateOfBirth = dateOfBirth,
    };

    public void SetPassword(string password) => PasswordHash = password;
    public int GetAge() => DateOfBirth.CalcularIdade();
}
