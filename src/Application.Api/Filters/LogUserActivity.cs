using Application.Core.Common.Dispatcher;
using Application.Core.Model.User;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Api.Filters;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        ActionExecutedContext resultContext = await next();

        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

        int userId = resultContext.HttpContext.User.GetUserId();

        _ = Task.Run(async () =>
        {
            try
            {
                using IServiceScope scope = context.HttpContext.RequestServices.CreateScope();
                RequestDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<RequestDispatcher>();
                await dispatcher.Dispatch<UpdateUserActivityModel, Result<bool>>(
                    new UpdateUserActivityModel(userId)
                );
            }
            catch { /* Error */ }
        });
    }
}
