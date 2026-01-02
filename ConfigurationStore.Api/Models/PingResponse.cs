using System.Text.Json.Serialization;

namespace ConfigurationStore.Api.Models;

public class PingResponse
{
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("message")]
    public string Message { get; set; } = "pong";
}