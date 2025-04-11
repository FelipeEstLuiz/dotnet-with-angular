using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics.CodeAnalysis;

namespace Application.Domain.Converter;

[ExcludeFromCodeCoverage]
public class CustomLongDateTimeConverter : IsoDateTimeConverter
{
    public CustomLongDateTimeConverter() => DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            writer.WriteNull();
        else
            base.WriteJson(writer, value, serializer);
    }
}
