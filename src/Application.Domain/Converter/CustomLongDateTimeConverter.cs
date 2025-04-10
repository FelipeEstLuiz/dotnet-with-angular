using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Application.Domain.Converter;

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
