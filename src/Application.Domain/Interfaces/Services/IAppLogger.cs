namespace Application.Domain.Interfaces.Services;

public interface IAppLogger<TClass>
{
    void LogDebug(string? message, params object?[] args);
    void LogInformation(string? message, params object?[] args);
    void LogError(System.Exception? exception, string? message, params object?[] args);

}
