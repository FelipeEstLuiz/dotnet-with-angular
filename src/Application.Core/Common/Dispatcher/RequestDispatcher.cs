using Application.Core.Services;
using Application.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Common.Dispatcher;

public class RequestDispatcher(IServiceProvider serviceProvider)
{
    public async Task<TResponse> Dispatch<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<IValidator<TRequest>> validators = serviceProvider.GetServices<IValidator<TRequest>>();
        RequestValidator<TRequest> validator = new(validators);
        await validator.ValidateAsync(request, cancellationToken);

        IRequestHandler<TRequest, TResponse> handler = serviceProvider
            .GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        List<IPipelineBehavior<TRequest, TResponse>> behaviors = [.. serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>()
            .Reverse()];

        Func<Task<TResponse>> next = () => handler.Handle(request, cancellationToken);

        foreach (IPipelineBehavior<TRequest, TResponse> behavior in behaviors)
        {
            Func<Task<TResponse>> current = next;
            next = () => behavior.Handle(request, cancellationToken, current);
        }

        return await next();
    }
}

