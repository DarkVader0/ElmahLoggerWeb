using AspNetCore.Authentication.ApiKey;

namespace ElmahLoggerWeb.Lambda.Authentication;

public class ApiKeyProvider : IApiKeyProvider
{
    private readonly IConfiguration _configuration;

    public ApiKeyProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IApiKey> ProvideAsync(string key)
    {
        return new ApiKey()
        {
            Key = _configuration.GetValue<string>("api_key"),
            OwnerName = "App"
        };
    }
}