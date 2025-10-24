using Application.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Core.Services;

public class AppLogger<TClass>(
    ILogger<TClass> logger,
    IHttpContextAccessor context
) : IAppLogger<TClass>
{
    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        using (LogContext.PushProperty("Protocol", GetProtocol()))
        {
            if (message is null)
                message = $"Protocol: {GetProtocol()}";
            else
                message += $" Protocol: {GetProtocol()}";

            logger.LogError(exception, message, args);
        }
    }

    public void LogDebug(string? message, params object?[] args)
    {
        if (message is null)
            message = $"Protocol: {GetProtocol()}";
        else
            message += $" Protocol: {GetProtocol()}";

        logger.LogDebug(message, args);
    }

    public void LogInformation(string? message, params object?[] args)
    {
        if (message is null)
            message = $"Protocol: {GetProtocol()}";
        else
            message += $" Protocol: {GetProtocol()}";

        logger.LogInformation(message, args);
    }

    private string? GetProtocol() => context.HttpContext.Request.Headers["Protocolo"].ToString();
}
