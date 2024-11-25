using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PassedPawn.BusinessLogic.Exceptions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.Models.Configuration;
using PassedPawn.Models.DTOs.Keycloak;

namespace PassedPawn.BusinessLogic.Services;

public class KeycloakService(IOptions<KeycloakConfig> keycloakConfig) : IKeycloakService
{
    public async Task<KeycloakRegistrationResponse> GetAccessTokenAsync()
    {
        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;
        var clientId = keycloakConfig.Value.ClientId;
        var clientSecret = keycloakConfig.Value.ClientSecret;
        var grandType = keycloakConfig.Value.GrandType;

        var formData = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", grandType }
        };

        var content = new FormUrlEncodedContent(formData);

        var client = new HttpClient();

        HttpResponseMessage response =
            await client.PostAsync($"{baseUrl}/realms/{realm}/protocol/openid-connect/token", content);

        KeycloakRegistrationResponse mappedResponse =
            await response.Content.ReadFromJsonAsync<KeycloakRegistrationResponse>()
            ?? throw new KeycloakNullResponseException();

        return mappedResponse;
    }
}