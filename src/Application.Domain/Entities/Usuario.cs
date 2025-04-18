﻿namespace Application.Domain.Entities;

public class Usuario : Entity
{
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


    public static Usuario Create(string nome, string email) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        NormalizedEmail = email.ToUpperInvariant(),
        UserName = nome,
        NormalizedUserName = nome.ToUpperInvariant(),
        SecurityStamp = Guid.NewGuid().ToString(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
        CriadoEm = DateTime.UtcNow
    };

    public void SetPassword(string password) => PasswordHash = password;
}
