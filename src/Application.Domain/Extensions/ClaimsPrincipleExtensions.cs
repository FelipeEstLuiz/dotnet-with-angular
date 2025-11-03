using System.Security.Claims;

namespace Application.Domain.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        string userName = user.FindFirstValue(ClaimTypes.Name) ?? throw new UnauthorizedAccessException();
        return userName;
    }

    public static int GetUserId(this ClaimsPrincipal user)
    {
        string userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        return int.Parse(userId);
    }
}
