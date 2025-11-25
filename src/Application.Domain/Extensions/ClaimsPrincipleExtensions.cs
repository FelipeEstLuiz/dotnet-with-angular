using System.Security.Claims;

namespace Application.Domain.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
}
