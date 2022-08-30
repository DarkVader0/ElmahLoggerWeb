using System.Text.Json.Serialization;
using FastEndpoints;

namespace ElmahLoggerWeb.Lambda.Contracts.Requests;

public class LogRequest
{
    [FromHeader("api_key")] 
    public string ApiKey { get; init; } = default!;
    
    [JsonPropertyName("message")]
    public string Message { get; init; } = default!;
}