using Application.Domain.Entities;
using Bogus;

namespace Tests.Fixtures;

internal static class GenerateUser
{
    public static User Created() => FakerUser().Generate();

    public static Faker<User> FakerUser() => new Faker<User>()
        .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
        .RuleFor(cmd => cmd.UserName, f => f.Person.FirstName)
        .RuleFor(cmd => cmd.FullName, f => f.Person.FullName)
        .RuleFor(cmd => cmd.NormalizedUserName, f => f.Person.FullName.ToUpperInvariant())
        .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
        .RuleFor(cmd => cmd.NormalizedEmail, f => f.Internet.Email().ToUpperInvariant())
        .RuleFor(u => u.DateOfBirth, f =>
        {
            DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
            return DateOnly.FromDateTime(date);
        })
        .RuleFor(u => u.Introduction, f => f.Lorem.Sentence(2000))
        .RuleFor(u => u.Interests, f => f.Lorem.Sentence(1000))
        .RuleFor(u => u.LookingFor, f => f.Lorem.Sentence(1000))
        .RuleFor(u => u.City, f => f.Address.City())
        .RuleFor(u => u.Country, f => f.Address.Country())
        .RuleFor(u => u.Gender, f => f.PickRandom("Male", "Female", "Not informed"))
        .RuleFor(u => u.Created, f => f.Date.Past(1, DateTime.UtcNow));
}
