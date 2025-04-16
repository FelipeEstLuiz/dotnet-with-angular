using FluentValidation;

namespace Application.Core.Services;

public class RequestValidator<TRequest>(IEnumerable<IValidator<TRequest>> validators)
{
    public async Task ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (!validators.Any()) return;

        List<FluentValidation.Results.ValidationFailure> failures = [.. (await Task.WhenAll(validators
            .Select(async v => await v.ValidateAsync(request, cancellationToken))))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .Distinct()];

        if (failures.Any())
            throw new ValidationException(failures);
    }
}
