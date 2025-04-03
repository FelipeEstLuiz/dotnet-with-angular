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

    //public Result<U> SetResult<U>(Func<TResponse, Result<U>> func)
    //    => IsSuccess ? func(Data!) : Result<U>.Failure(Errors);

    public Result<U> SetResult<U>(Func<TResponse, U> func)
    {
        if (IsSuccess)
        {
            Result<U> result = func(Data!);
            return result;
        }

        return Result<U>.Failure(Errors, ResponseCode);
    }

    public Result<U> SetResult<U>() => Result<U>.Failure(Errors, ResponseCode);

    public static Result<TResponse> Failure(string message) => Failure(message, ResponseCodes.NONE);

    public static Result<TResponse> Failure(string message, ResponseCodes responseCode)
        => new Result<TResponse>(false).AddError(message, responseCode);

    public static Result<TResponse> Failure(
        IEnumerable<string> messages,
        ResponseCodes responseCode
    ) => new Result<TResponse>(false).AddError(messages, responseCode);

    public static Result<TResponse> Failure(IEnumerable<string> messages)
        => Failure(messages, ResponseCodes.NONE);

    private Result<TResponse> AddError(string message, ResponseCodes responseCode)
    {
        AddError(message);
        ResponseCode = responseCode;
        return this;
    }

    private Result<TResponse> AddError(string message)
    {
        if (!_messages.Contains(message))
            _messages.Add(message);

        return this;
    }

    private Result<TResponse> AddError(
        IEnumerable<string> messages,
        ResponseCodes responseCode
    )
    {
        foreach (string message in messages)
            AddError(message, responseCode);

        return this;
    }

    public override string ToString() => IsSuccess
        ? "Success"
        : string.Join("; ", _messages);
}
