using Swashbuckle.AspNetCore.Annotations;
using System.Collections.ObjectModel;

namespace Application.Api.Controllers._Shared;

public class Response(string protocol)
{
    private readonly IList<string> _messages = [];

    [SwaggerSchema(ReadOnly = true, Description = "Indicates if the operation was successful.")]
    public bool Success => Errors?.Any() == false;

    [SwaggerSchema(ReadOnly = true, Description = "List of error messages.")]
    public IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);

    [SwaggerSchema(Description = "Data returned by the operation.")]
    public object? Data { get; set; }

    [SwaggerSchema(ReadOnly = true, Description = "Operation protocol identifier.")]
    public string Protocol => protocol;

    public Response(object? data, string protocol) : this(protocol) => Data = data;

    public void AddError(string message, params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0)
            message = string.Format(message, parameters);

        if (!Errors.Contains(message))
            _messages.Add(message);
    }

    public void AddError(IEnumerable<string> errors, params object[] parameters)
    {
        foreach (string message in errors)
            AddError(message, parameters);
    }
}

public class Response<TResponse>
{
    public bool Success { get; set; } = true;
    public required TResponse Data { get; set; }
    public string? Protocol { get; set; }
}


public class ResponseError
{
    public bool Success { get; set; } = false;
    public object? Data { get; set; } = null;
    public required IEnumerable<string> Errors { get; set; }
    public string? Protocol { get; set; }
}
