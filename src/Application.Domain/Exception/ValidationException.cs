using System.Net;

namespace Application.Domain.Exception;

public static class ValidationExtensions
{
    public static void ThrowIfNull<T>(this T obj, string paramName) where T : class
    {
        ValidationException.When(
            obj == null,
            $"{paramName} nao pode ser nulo."
        );
    }

    public static void ThrowIfNullOrEmpty(this string str, string paramName)
    {
        ValidationException.When(
            string.IsNullOrWhiteSpace(str),
            $"{paramName} nao pode ser nulo ou vazio."
        );
    }

    public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> collection, string paramName)
    {
        ValidationException.When(
            collection == null || !collection.Any(),
            $"{paramName} nao pode ser nulo ou vazio."
        );
    }

    public static void ThrowIfFalse(this bool condition, string message)
    {
        ValidationException.When(
            !condition,
            message
        );
    }
}

public class ValidationException(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    : System.Exception(error)
{
    public HttpStatusCode StatusCode { get; } = httpStatusCode;

    public static void When(bool hasError, string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        if (hasError)
            throw new ValidationException(error, httpStatusCode);
    }
}