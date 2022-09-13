using Amazon.Lambda.Core;
using AspNetCore.Authentication.ApiKey;
using Elmah.Io.Client;
using ElmahLoggerWeb.Lambda.Authentication;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Endpoints
builder.Services.AddFastEndpoints();

// Making it lambda
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var logger = ElmahioAPI.Create(configuration["ElmahIo:ApiKey"]);
logger.Messages.OnMessage += (sender, eventArgs) =>
{
    eventArgs.Message.Application = configuration["ElmahIo:Application"];
    LambdaLogger.Log("logged message");
};

builder.Services.AddSingleton<IElmahioAPI>(logger);

// Auth
builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
    .AddApiKeyInHeader<ApiKeyProvider>(opt =>
    {
        opt.Realm = "Elmah logger";
        opt.KeyName = "api_key";
        opt.IgnoreAuthenticationIfAllowAnonymous = true;
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