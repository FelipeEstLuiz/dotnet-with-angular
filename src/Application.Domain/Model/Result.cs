using Application.Domain.Enums;
using System.Collections.ObjectModel;

namespace Application.Domain.Model;

public class Result<TResponse>(bool isSuccess)
{
    private readonly IList<string> _messages = [];

    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);
    public TResponse? Data { get; private set; }
    public ResponseCodes ResponseCode { get; private set; } = ResponseCodes.NONE;

    public static Result<TResponse> Success(TResponse data) => new(true) { Data = data };

    public static implicit operator Result<TResponse>(TResponse value) => Success(value);

    public Result<U> SetResult<U>(Func<TResponse, Result<U>> func)
        => IsSuccess ? func(Data!) : Result<U>.Failure(Errors);

    public static Result<TResponse> Failure(
        string message,
        params object[] parameters
    ) => Failure(message, ResponseCodes.NONE, parameters);

    public static Result<TResponse> Failure(string message, ResponseCodes responseCode, params object[] parameters)
        => new Result<TResponse>(false).AddError(message, responseCode, parameters);

    public static Result<TResponse> Failure(
        IEnumerable<string> messages,
        ResponseCodes responseCode,
        params object[] parameters
    ) => new Result<TResponse>(false).AddError(messages, responseCode, parameters);

    public static Result<TResponse> Failure(IEnumerable<string> messages, params object[] parameters)
        => Failure(messages, ResponseCodes.NONE, parameters);

    private Result<TResponse> AddError(string message, ResponseCodes responseCode, params object[] parameters)
    {
        AddError(message, parameters);
        ResponseCode = responseCode;
        return this;
    }

    private Result<TResponse> AddError(string message, params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0)
            message = string.Format(message, parameters);

        if (!_messages.Contains(message))
            _messages.Add(message);

        return this;
    }

    private Result<TResponse> AddError(
        IEnumerable<string> messages,
        ResponseCodes responseCode,
        params object[] parameters
    )
    {
        foreach (string message in messages)
            AddError(message, responseCode, parameters);

        return this;
    }

    public override string ToString() => IsSuccess
        ? "Success"
        : string.Join("; ", _messages);
}
