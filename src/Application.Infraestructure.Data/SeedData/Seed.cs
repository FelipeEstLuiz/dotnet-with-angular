using Application.Domain.Entities;
using Application.Infraestructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Infraestructure.Data.SeedData;

public class Seed
{
    public static async Task SeedUsers(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        string path = Path.Combine(AppContext.BaseDirectory, @"SeedData\UserSeed.json");

        string userData = await File.ReadAllTextAsync(path);

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true
        };

        List<User>? users = JsonSerializer.Deserialize<List<User>>(userData, option);

        if (users == null) return;

        foreach (User user in users)
        {
            user.Created = DateTime.UtcNow;
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);

            PasswordHasher<User> hasher = new();
            user.SetPassword(hasher.HashPassword(user, "Pas$w0rd"));
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
