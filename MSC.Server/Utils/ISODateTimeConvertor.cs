using System.Text.Json;
using System.Text.Json.Serialization;
namespace MSC.Server.Utils;

sealed class ISODateTimeConvertor : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString() ?? "1970-01-01T00:00:00.0000000Z");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("O"));
    }
}