using System.Text.Json.Serialization;

namespace PassedPawn.Models.DTOs.Keycloak;

public class KeyclockRegistrationResponse
{
    [JsonPropertyName("access_token")]
    public string? Token { get; init; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }
    
    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }
    
    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; init; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; init; }
}