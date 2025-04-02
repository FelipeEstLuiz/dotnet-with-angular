using Application.Api.Controllers._Shared;
using Application.Domain.Exception;
using Newtonsoft.Json;
using System.Net;

namespace Application.Api.Middleware;

public class GlobalExceptionHandlerMiddleware(CommunicationProtocol communicationProtocol) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        IEnumerable<string> erros = [];

        if (exception is FluentValidation.ValidationException validationException)
        {
            httpStatusCode = HttpStatusCode.BadRequest;

            foreach (FluentValidation.Results.ValidationFailure failure in validationException.Errors)
            {
                string message = $"{failure.PropertyName} | {failure.ErrorMessage} | Valor: {failure.AttemptedValue}";
                erros = [.. erros, message];
            }
        }
        else if (exception is UnauthorizedAccessException)
        {
            httpStatusCode = HttpStatusCode.Unauthorized;
            erros = ["Usuário nao autorizado"];
        }
        else if (exception is ValidationException validacaoException)
        {
            httpStatusCode = validacaoException.StatusCode;
            erros = [validacaoException.Message];
        }
        else
        {
            erros = ["Erro ao processar requisição"];
        }

        context.Response.StatusCode = (int)httpStatusCode;

        Response baseResponse = new(communicationProtocol.ToString());
        baseResponse.AddError(erros);

        JsonSerializerSettings settings = new()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(baseResponse, settings));
    }
}
