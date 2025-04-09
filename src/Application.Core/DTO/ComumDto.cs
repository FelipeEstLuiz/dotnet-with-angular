using Application.Domain.Converter;

namespace Application.Core.DTO;

public record ComumDto
{
    public Guid Id { get; internal set; }

    [Newtonsoft.Json.JsonConverter(typeof(CustomLongDateTimeConverter))]
    public DateTime DataCadastro { get; internal set; }
}
