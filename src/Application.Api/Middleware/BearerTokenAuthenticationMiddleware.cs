using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Application.Api.Middleware;

public class BearerTokenAuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (ValidateAuthorizationHeader(context))
            await next(context);
        else
            throw new UnauthorizedAccessException();
    }

    private static bool ValidateAuthorizationHeader(HttpContext context)
    {
        if (context is null) return false;

        ControllerActionDescriptor? actionDescriptor = context
            .GetEndpoint()?
            .Metadata
            .GetMetadata<ControllerActionDescriptor>();

        // Verifica se o atributo AllowAnonymousAttribute está presente no descritor da ação
        bool isAllowAnonymous = actionDescriptor?
            .EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)) == true;

        if (isAllowAnonymous) return true;

        string? authorizationHeader = context.Request.Headers.Authorization;
        return !string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ");
    }
}

