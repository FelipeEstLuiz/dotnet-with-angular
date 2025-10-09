using Application.Domain.Extensions;
using Application.Domain.VO;

namespace Application.Core.DTO.User;

public record UserDto : ComumDto
{
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public int Age { get; set; }
    public string KnowAs { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime LastActive { get; set; }
    public List<PhotoUserDto>? Photo { get; set; }

    public static UserDto Map(UserVo usuario) => new()
    {
        Email = usuario.Email,
        Name = usuario.Name,
        Id = usuario.Id,
        Created = usuario.Created,
        DateOfBirth = usuario.DateOfBirth,
        Age = usuario.DateOfBirth.CalcularIdade(),
        Photo = usuario.Photos?.Select(x => new PhotoUserDto(x.Id, x.Url, x.IsMain)).ToList(),
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
