using ElmahLoggerWeb.Lambda.Contracts.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ElmahLoggerWeb.Lambda.Endpoints;

[HttpPost("/log/error"), Authorize]
public class LogEndpoint : Endpoint<LogRequest>
{
    private readonly ILoggerFactory _logger;

    public LogEndpoint(ILoggerFactory logger)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(LogRequest req, CancellationToken ct)
    {
        var log = _logger.CreateLogger("LogEndpoint");
        log.LogError(req.Message);
        
        
        await SendNoContentAsync(cancellation: ct);
        Thread.Sleep(3500);
    }
}