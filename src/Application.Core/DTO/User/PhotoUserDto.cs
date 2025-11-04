namespace Application.Core.DTO.User;

public record PhotoUserDto(
    int Id,
    string Url,
    string? PublicId,
    int? MemberId
);
