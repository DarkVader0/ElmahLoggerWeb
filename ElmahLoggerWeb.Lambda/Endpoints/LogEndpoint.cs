using ElmahLoggerWeb.Lambda.Contracts.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ElmahLoggerWeb.Lambda.Endpoints;

[HttpPost("/log/error"), Authorize]
public class LogEndpoint : Endpoint<LogRequest>
{
    private readonly ILogger<LogEndpoint> _logger;

    public LogEndpoint(ILogger<LogEndpoint> logger)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(LogRequest req, CancellationToken ct)
    {
        _logger.LogError(req.Message);

        await SendNoContentAsync(cancellation: ct);
    }
}