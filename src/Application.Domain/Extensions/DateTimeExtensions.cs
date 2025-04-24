namespace Application.Domain.Extensions;

public static class DateTimeExtensions
{
    public static int CalcularIdade(this DateOnly dateOfBirth)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

        int age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age)) age--;

        return age;
    }
}
