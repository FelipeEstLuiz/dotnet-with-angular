namespace Application.Core.DTO.User;

public record UserDto : ComumDto
{
    public string Name { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime LastActive { get; set; }

    public static UserDto Map(Domain.Entities.User user) => new()
    {
        Email = user.Email!,
        Name = user.UserName!,
        FullName = user.FullName,
        Id = user.Id,
        Created = user.Created,
        DateOfBirth = user.DateOfBirth,
        Introduction = user.Introduction,
        City = user.City,
        Country = user.Country,
        LastActive = user.LastActive,
        Gender = user.Gender,
        Interests = user.Interests,
        LookingFor = user.LookingFor,
        PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
    };
}
