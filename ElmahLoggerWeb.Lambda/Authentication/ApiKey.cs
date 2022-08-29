using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

namespace ElmahLoggerWeb.Lambda.Authentication;

public class ApiKey : IApiKey
{
    public string Key { get; init; } = default!;
    public string OwnerName { get; init; } = default!;
    public IReadOnlyCollection<Claim> Claims { get; init; } = new List<Claim>();
}