using AspNetCore.Authentication.ApiKey;
using Elmah.Io.Extensions.Logging;
using ElmahLoggerWeb.Lambda.Authentication;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Endpoints
builder.Services.AddFastEndpoints();

// Making it lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Auth
builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
    .AddApiKeyInHeader<ApiKeyProvider>(opt =>
    {
        opt.Realm = "Elmah logger";
        opt.KeyName = "api_key";
        opt.IgnoreAuthenticationIfAllowAnonymous = true;
    });

// Logger
builder.Host.ConfigureLogging((ctx, logging) =>
{
    // logging.Services.Configure<ElmahIoProviderOptions>(ctx.Configuration.GetSection("ElmahIo"));
    logging.AddElmahIo(opt =>
    {
        opt.ApiKey = config["ElmahIo:ApiKey"];
        opt.LogId = Guid.Parse(config["ElmahIo:LogId"]);
        opt.Application = config["ElmahIo:Application"];
        
    });
    logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Error);
});

var app = builder.Build();

// Https redirection
app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Fast endpoints
app.UseFastEndpoints();

// Swagger
app.UseOpenApi();
app.UseSwaggerUi3(s =>
{
    s.ConfigureDefaults();
});


app.Run();