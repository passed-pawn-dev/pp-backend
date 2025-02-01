using System.Text.Json.Serialization;

namespace PassedPawn.Models.Keyclock;

public class ResourceAccess
{
    [JsonPropertyName("account")]
    public Account? Account { get; init; }
    
    [JsonPropertyName("api-client")]
    public ApiClient? ApiClient { get; init; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class Account
{
    [JsonPropertyName("roles")]
    public string[]? Roles { get; init; }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiClient
{
    [JsonPropertyName("roles")]
    public string[]? Roles { get; init; }
}