using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics.CodeAnalysis;

namespace Application.Domain.Converter;

[ExcludeFromCodeCoverage]
public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter() => DateTimeFormat = "dd/MM/yyyy";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            writer.WriteNull();
        else
            base.WriteJson(writer, value, serializer);
    }
}
