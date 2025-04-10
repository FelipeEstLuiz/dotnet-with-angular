using Application.Core.Services;
using Application.Domain.Exception;
using Application.Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Api.Filter;

public class CustomAuthorizationFilter(IConfiguration configuration) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);

        bool hasAllowAnonymous = context
            .ActionDescriptor
            .EndpointMetadata
            .Any(em => em is AllowAnonymousAttribute);

        if (!hasAllowAnonymous)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization") == false)
                throw new UnauthorizedAccessException();

            string authorizationToken = context.HttpContext.Request.Headers.Authorization.ToString();

            if (!authorizationToken.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException();

            authorizationToken = authorizationToken.Replace("Bearer ", "");

            //if (!ValidateJwtToken(authorizationToken))
            //    await currentUserService.SetUserDataByToken(authorizationToken);
        }

        await next();
    }

    private bool ValidateJwtToken(string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(ClsGlobal.GetTokenKey(configuration))
            };

            System.Security.Claims.ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken validatedToken
            );

            return validatedToken is not JwtSecurityToken jwtSecurityToken 
                || jwtSecurityToken.ValidTo >= DateTime.UtcNow;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
