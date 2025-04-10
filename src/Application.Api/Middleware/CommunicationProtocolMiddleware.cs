using Application.Api.Controllers._Shared;
using Application.Domain.Util;
using System.Reflection;

namespace Application.Api.Middleware;

public class CommunicationProtocolMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, CommunicationProtocol protocol)
    {
        string? version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        protocol.SetProtocol(ProtocolGenerator.SetProtocol());

        context.Response.Headers["X-Assembly-Version"] = version;
        context.Request.Headers["X-Assembly-Version"] = version;
        context.Request.Headers["Protocolo"] = protocol.ToString();
        context.Response.Headers["Protocolo"] = protocol.ToString();
        await next(context);
    }
}
