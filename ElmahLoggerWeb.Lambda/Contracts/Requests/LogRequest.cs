using Microsoft.AspNetCore.Mvc;

namespace ElmahLoggerWeb.Lambda.Contracts.Requests;

public class LogRequest
{
    [FastEndpoints.FromHeader("api_key")] public string ApiKey { get; init; } = default!;
    public string Message { get; init; } = default!;
}