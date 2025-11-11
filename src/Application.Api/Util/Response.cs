using Swashbuckle.AspNetCore.Annotations;
using System.Collections.ObjectModel;
using System.Net;

namespace Application.Api.Util;

public struct Response
{
    private readonly IList<string> _messages = [];

    [SwaggerSchema(ReadOnly = true, Description = "Indicates if the operation was successful.")]
    public readonly bool Success => Errors?.Any() == false;

    [SwaggerSchema(ReadOnly = true, Description = "List of error messages.")]
    public readonly IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);

    [SwaggerSchema(Description = "Data returned by the operation.")]
    public object? Data { get; private set; }

    [SwaggerSchema(Description = "Status code.")]
    public int StatusCode { get; private set; }

    [SwaggerSchema(ReadOnly = true, Description = "Operation protocol identifier.")]
    public string Protocol { get; private set; }

    [SwaggerSchema(ReadOnly = true, Description = "Total items.")]
    public int TotalItems { get; set; }

    [SwaggerSchema(ReadOnly = true, Description = "Current page.")]
    public int CurrentPage { get; set; }

    [SwaggerSchema(ReadOnly = true, Description = "Total pages.")]
    public int TotalPages { get; set; }

    [SwaggerSchema(ReadOnly = true, Description = "Page size.")]
    public int PageSize { get; set; }

    private Response(
        object? data,
        string protocol,
        HttpStatusCode statusCode,
        int totalItems = 0,
        int currentPage = 0,
        int totalPages = 0,
        int pageSize = 0
    ) : this(
        protocol: protocol,
        statusCode: statusCode,
        totalItems: totalItems,
        currentPage: currentPage,
        totalPages: totalPages,
        pageSize: pageSize
    ) => Data = data;

    private Response(
        string protocol,
        HttpStatusCode statusCode,
        int totalItems = 0,
        int currentPage = 0,
        int totalPages = 0,
        int pageSize = 0
    )
    {
        Protocol = protocol;
        StatusCode = (int)statusCode;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
    }

    public static Response ResponseSuccess(
        object? data,
        string protocol,
        HttpStatusCode statusCode,
        int totalItems = 0,
        int currentPage = 0,
        int totalPages = 0,
        int pageSize = 0
    ) => new(
        data: data,
        protocol: protocol,
        statusCode: statusCode,
        totalItems: totalItems,
        currentPage: currentPage,
        totalPages: totalPages,
        pageSize: pageSize
    );

    public static Response Failure(
        string protocol,
        IEnumerable<string> errors,
        HttpStatusCode statusCode,
        params object[] parameters
    ) => new Response(data: null, protocol: protocol, statusCode: statusCode).AddError(errors, parameters);

    public static Response Failure(
        string protocol,
        string error,
        HttpStatusCode statusCode,
        params object[] parameters
    ) => new Response(data: null, protocol: protocol, statusCode: statusCode).AddError(error, parameters);

    private readonly Response AddError(string message, params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0)
            message = string.Format(message, parameters);

        if (!Errors.Contains(message))
            _messages.Add(message);

        return this;
    }

    private readonly Response AddError(IEnumerable<string> errors, params object[] parameters)
    {
        foreach (string message in errors)
            AddError(message, parameters);

        return this;
    }
}

public record Response<TResponse>
{
    public bool Success { get; set; } = true;
    public required TResponse Data { get; set; }
    public string? Protocol { get; set; }
    public int StatusCode { get; set; }
}

public record ResponseServerSide<TResponse>
{
    public bool Success { get; set; } = true;
    public required TResponse Data { get; set; }
    public string? Protocol { get; set; }
    public int StatusCode { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}

public record ResponseError
{
    public bool Success { get; set; } = false;
    public required IEnumerable<string> Errors { get; set; }
    public string? Protocol { get; set; }
    public int StatusCode { get; set; }
}
