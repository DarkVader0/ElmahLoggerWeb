using System.Dynamic;
using Elmah.Io.Client;
using ElmahLoggerWeb.Lambda.Contracts.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ElmahLoggerWeb.Lambda.Endpoints;

[HttpPost("/log/error"), Authorize]
public class LogEndpoint : Endpoint<LogRequest>
{
    private readonly IElmahioAPI _logger;
    private readonly string _logId;

    public LogEndpoint(IConfiguration configuration, IElmahioAPI elmahioApi)
    {
        _logger = elmahioApi;
        _logId = configuration["ElmahIo:LogId"];
    }

    public override async Task HandleAsync(LogRequest req, CancellationToken ct)
    {
        await _logger.Messages.CreateAsync(_logId, new CreateMessage
        {
            Title = req.Message,
            Severity = "Error"
        }, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}