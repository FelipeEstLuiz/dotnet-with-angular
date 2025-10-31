using Application.Api.Util;
using Application.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Application.Api.Middleware;

public class CustomAuthResultHandler(IAppLogger<CustomAuthResultHandler> logger) : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult
    )
    {
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(Response.Failure(
                GetProtocol(context),
                ["Acesso negado"],
                System.Net.HttpStatusCode.Forbidden
            ));
            return;
        }

        if (authorizeResult.Challenged)
        {
            logger.LogInformation("Unauthorized user");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(Response.Failure(
                GetProtocol(context),
                ["Unauthorized user"],
                System.Net.HttpStatusCode.Unauthorized
            ));
            return;
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }

    private static string GetProtocol(HttpContext context)
    {
        CommunicationProtocol protocol = context.RequestServices.GetRequiredService<CommunicationProtocol>();
        return protocol.ToString();
    }
}
