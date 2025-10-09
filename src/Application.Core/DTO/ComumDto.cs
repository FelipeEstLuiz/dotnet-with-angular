using Application.Domain.Converter;

namespace Application.Core.DTO;

public record ComumDto
{
    public int Id { get; internal set; }

    [Newtonsoft.Json.JsonConverter(typeof(CustomLongDateTimeConverter))]
    public DateTime Created { get; internal set; }
}
