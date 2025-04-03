using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Application.Domain.Converter;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter() => DateTimeFormat = "yyyy-MM-dd";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
            writer.WriteNull();
        else
            base.WriteJson(writer, value, serializer);
    }
}
