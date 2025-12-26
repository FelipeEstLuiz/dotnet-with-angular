namespace Application.Core.DTO.Admin;

public record UsersRolesDto(string Id, string Email, IEnumerable<string> Roles);
