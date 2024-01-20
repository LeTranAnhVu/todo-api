using System.Text.Json.Serialization;

namespace Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RepeatableType
{
    Once = 1,
    Daily
}
