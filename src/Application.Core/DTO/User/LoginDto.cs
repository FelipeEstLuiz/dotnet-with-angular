namespace Application.Core.DTO.User;

public record LoginDto(string Id, string Name, string FullName, string Email, string? ImageUrl, string Token)
{
    public static LoginDto Map(Domain.Entities.User user, string token) => new(
        user.Id,
        user.UserName!,
        user.FullName,
        user.Email!,
        user.ImageUrl,
        token
    );
}

public record UserLoginDto(LoginDto Login, string RefreshToken);
