using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        List<FluentValidation.Results.ValidationFailure> failures = (await Task.WhenAll(validators
            .Select(async v => await v.ValidateAsync(request))))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .Distinct()
            .ToList();

        return failures.Any()
            ? throw new ValidationException(failures)
            : await next();
    }
}