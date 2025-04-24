using Application.Domain.Entities;
using Bogus;

namespace Tests.Fixtures;

internal static class GenerateUser
{
    public static User Created() => FakerUser().Generate();

    public static Faker<User> FakerUser() => new Faker<User>()
        .RuleFor(cmd => cmd.Id, f => f.Random.Guid())
        .RuleFor(cmd => cmd.UserName, f => f.Person.FullName)
        .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
        .RuleFor(u => u.DateOfBirth, f =>
        {
            DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
            return DateOnly.FromDateTime(date);
        })
        .RuleFor(u => u.Introduction, f => f.Lorem.Sentence(2000))
        .RuleFor(u => u.Gender, f => f.PickRandom("Masculino", "Feminino", "Outro"))
        .RuleFor(u => u.KnowAs, f => f.Name.FirstName())
        .RuleFor(u => u.Created, f => f.Date.Past(1, DateTime.UtcNow));
}
