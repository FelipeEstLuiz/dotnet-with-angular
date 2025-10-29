namespace Application.Core.DTO.User;

public record PhotoUserDto(
    int Id,
    string Url,
    bool IsMain,
    string? PublicId,
    int? MemberId
);
