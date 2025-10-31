namespace Application.Core.DTO.User;

public record UserDto : ComumDto
{
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string KnowAs { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime LastActive { get; set; }

    public static UserDto Map(Domain.Entities.User usuario) => new()
    {
        Email = usuario.Email,
        Name = usuario.UserName,
        Id = usuario.Id,
        Created = usuario.Created,
        DateOfBirth = usuario.DateOfBirth,
        Introduction = usuario.Introduction,
        City = usuario.City,
        Country = usuario.Country,
        LastActive = usuario.LastActive,
        Gender = usuario.Gender,
        Interests = usuario.Interests,
        KnowAs = usuario.KnowAs,
        LookingFor = usuario.LookingFor,
        PhotoUrl = usuario.Photos?.FirstOrDefault(x => x.IsMain)?.Url
    };
}
