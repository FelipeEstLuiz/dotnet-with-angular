using Application.Domain.Entities;
using Application.Infraestructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Infraestructure.Data.SeedData;

public static class Seed
{
    private static JsonSerializerOptions Options => new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedUsers(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        string path = Path.Combine(AppContext.BaseDirectory, @"SeedData\UserSeed.json");

        if (!File.Exists(path)) return;

        string userData = await File.ReadAllTextAsync(path);

        List<User>? users = JsonSerializer.Deserialize<List<User>>(userData, Options);

        if (users == null) return;

        foreach (User user in users)
        {
            user.Created = DateTime.UtcNow;
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);

            PasswordHasher<User> hasher = new();
            user.SetPassword(hasher.HashPassword(user, user.PasswordHash ?? "Pas$w0rd"));
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
