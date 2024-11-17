using System.Text.Json.Serialization;

namespace PassedPawn.Models.DTOs.Keycloak;

public class KeycloakRegistrationResponse
{
    [JsonPropertyName("access_token")]
    public required string Token { get; init; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }
    
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }
    
    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; init; }
    
    [JsonPropertyName("scope")]
    public required string Scope { get; init; }
}