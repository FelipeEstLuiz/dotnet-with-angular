using Application.Domain.Entities;
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

    public static async Task SeedUsers(UserManager<User> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        string path = Path.Combine(AppContext.BaseDirectory, @"SeedData\UserSeed.json");

        if (!File.Exists(path)) return;

        string userData = await File.ReadAllTextAsync(path);

        List<User>? users = JsonSerializer.Deserialize<List<User>>(userData, Options);

        if (users == null) return;

        foreach (User user in users)
        {
            user.Created = DateTime.UtcNow;
            user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
            user.SetName(user.FullName);
            user.SetEmail(user.Email);

            bool isMyUser = user.FullName == "Felipe Estevam Luiz";

            IdentityResult result = await userManager.CreateAsync(user, isMyUser ? "4389@#02Fel13" : "Pas$w0rd");

            if (!result.Succeeded)
                Console.WriteLine(result.Errors.First().Description);

            if (isMyUser)
                await userManager.AddToRolesAsync(user, ["Admin", "Moderator"]);

            await userManager.AddToRoleAsync(user, "User");
        }

        User admin = User.Create(
            fullName: "admin",
            email: "admin@admin.com",
            gender: "admin",
            dateOfBirth: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            introduction: null,
            interests: null,
            lookingFor: null,
            city: "Admin",
            country: "Admin"
        );

        await userManager.CreateAsync(admin, "Pas$w0rd");

        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
}
