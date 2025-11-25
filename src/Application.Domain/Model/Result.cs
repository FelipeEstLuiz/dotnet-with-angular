using Application.Domain.Enums;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Application.Domain.Model;

public static class Result
{
    public static Result<T> Success<T>(
        T data,
        int totalItems = 0,
        int currentPage = 0,
        int totalPages = 0,
        int pageSize = 0,
        ResponseCodes responseCode = ResponseCodes.NONE
    ) => Result<T>.Success(
        data,
        totalItems: totalItems,
        currentPage: currentPage,
        totalPages: totalPages,
        pageSize: pageSize,
        responseCode: responseCode
    );
    public static Result<T> Failure<T>(string message, ResponseCodes responseCode = ResponseCodes.NONE)
        => Result<T>.Failure(message, responseCode);

    public static Result<T> Failure<T>(IEnumerable<string> messages, ResponseCodes responseCode = ResponseCodes.NONE)
        => Result<T>.Failure(messages, responseCode);

    public static Result<bool> IsSuccess() => Result<bool>.Success(true);
    public static Result<bool> IsFailure(string message, ResponseCodes responseCode = ResponseCodes.NONE)
        => Result<bool>.Failure(message, responseCode);
    public static Result<bool> IsFailure(IEnumerable<string> messages, ResponseCodes responseCode = ResponseCodes.NONE)
        => Result<bool>.Failure(messages, responseCode);
    public static Result<bool> Try(bool condition, string message, ResponseCodes responseCode = ResponseCodes.NONE)
        => condition ? IsSuccess() : IsFailure(message, responseCode);
}

public class Result<TResponse>(bool isSuccess)
{
    private readonly IList<string> _messages = [];
    private TResponse? _data;
    public bool IsSuccess { get; private set; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TResponse? Data
    {
        get => IsSuccess ? _data : default;
        private set => _data = value;
    }
    public ResponseCodes ResponseCode { get; private set; } = ResponseCodes.NONE;

    public int TotalItems { get; set; } = 0;
    public int CurrentPage { get; set; } = 0;
    public int TotalPages { get; set; } = 0;
    public int PageSize { get; set; } = 0;

    public static Result<TResponse> Success(
        TResponse data,
        int totalItems = 0,
        int currentPage = 0,
        int totalPages = 0,
        int pageSize = 0,
        ResponseCodes responseCode = ResponseCodes.NONE
    ) => new(true)
    {
        Data = data,
        TotalItems = totalItems,
        CurrentPage = currentPage,
        TotalPages = totalPages,
        PageSize = pageSize,
        ResponseCode = responseCode
    };

    // ---------------------
    // IMPLICIT OPERATOR
    // ---------------------
    public static implicit operator Result<TResponse>(TResponse value) => Success(value);

    // ---------------------
    // FACTORY METHODS
    // ---------------------
    public static Result<TResponse> Failure(string message) => Failure(message, ResponseCodes.NONE);

    public static Result<TResponse> Failure(string message, ResponseCodes responseCode)
        => new Result<TResponse>(false).AddError(message, responseCode);

    public static Result<TResponse> Failure(
        IEnumerable<string> messages,
        ResponseCodes responseCode
    ) => new Result<TResponse>(false).AddError(messages, responseCode);

    public static Result<TResponse> Failure(IEnumerable<string> messages)
        => Failure(messages, ResponseCodes.NONE);


    // ---------------------
    // ADD ERROR
    // ---------------------
    private Result<TResponse> AddError(string message, ResponseCodes responseCode = ResponseCodes.NONE)
    {
        if (!_messages.Contains(message))
            _messages.Add(message);

        ResponseCode = responseCode;
        IsSuccess = false;

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

    // ---------------------
    // FUNCTIONAL HELPERS
    // ---------------------
    public TResult Match<TResult>(Func<TResponse?, TResult> onSuccess, Func<IEnumerable<string>, TResult> onFailure)
        => IsSuccess ? onSuccess(Data) : onFailure(Errors);

    public TResult Match<TResult>(Func<TResponse?, TResult> onSuccess, Func<IEnumerable<string>, ResponseCodes, TResult> onFailure)
       => IsSuccess ? onSuccess(Data) : onFailure(Errors, ResponseCode);

    public Result<U> Map<U>(Func<TResponse?, U> mapper, ResponseCodes responseCode = ResponseCodes.NONE)
    {
        if (IsFailure) return Result<U>.Failure(Errors, responseCode);
        try
        {
            return Result<U>.Success(
                mapper(Data),
                totalItems: TotalItems,
                currentPage: CurrentPage,
                totalPages: TotalPages,
                pageSize: PageSize
            );
        }
        catch (System.Exception ex)
        {
            return Result<U>.Failure(ex.Message, responseCode);
        }
    }

    public Result<U> Bind<U>(Func<TResponse?, Result<U>> binder)
        => IsFailure ? Result<U>.Failure(Errors, ResponseCode) : binder(Data);

    public Result<U> Bind<U>(Func<TResponse?, ResponseCodes, Result<U>> binder)
        => IsFailure ? Result<U>.Failure(Errors, ResponseCode) : binder(Data, ResponseCode);

    public Result<TResponse> AddErrorIf(bool condition, string message) => condition ? AddError(message) : this;
    public Result<TResponse> AddErrorIf(bool condition, string message, ResponseCodes responseCode) => condition ? AddError(message, responseCode) : this;

    public static Result<TResponse> Try(Func<TResponse> func)
    {
        try
        {
            return Success(func());
        }
        catch (System.Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    // ---------------------
    // SERIALIZATION
    // ---------------------
    public override string ToString()
    {
        if (IsSuccess)
        {
            try
            {
                return JsonConvert.SerializeObject(Data, Formatting.None);
            }
            catch (System.Exception ex)
            {
                return $"[Serialization Error] {ex.Message}";
            }
        }

        return $"{string.Join("; ", _messages)} - ResponseCode: {ResponseCode}";
    }
}
