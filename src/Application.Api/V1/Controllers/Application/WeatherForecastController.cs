using Application.Api.Controllers._Shared;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.V1.Controllers.Application;

[ApiExplorerSettings(GroupName = "Contador")]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(Response))]
[ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(Response))]
public class WeatherForecastController(CommunicationProtocol protocol) : BaseApplicationController(protocol)
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<IEnumerable<WeatherForecast>>))]
    public IActionResult Get()
    {
        return HandlerResponse(
            HttpStatusCode.OK,
            Result<IEnumerable<WeatherForecast>>.Success(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })));
    }
}
