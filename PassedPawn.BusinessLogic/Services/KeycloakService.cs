using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using PassedPawn.BusinessLogic.Exceptions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.Models.Configuration;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.User;
using PassedPawn.Models.DTOs.User.Coach;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services;

public class KeycloakService(IOptions<KeycloakConfig> keycloakConfig, IMapper mapper) : IKeycloakService
{
    public async Task<HttpResponseMessage> RegisterUserInKeycloak<T>(T dto) where T : UserUpsertDto
    {
        var userRegistrationDto = mapper.Map<UserRegistrationDto>(dto);
        
        var role = dto switch
        {
            CoachUpsertDto _ => "coach",
            StudentUpsertDto _ => "student",
            _ => throw new ArgumentException($"Unsupported DTO type: {dto.GetType().Name}")
        };
        
        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;
        
        var accessTokenResponse = await GetAccessTokenAsync();

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessTokenResponse.Token);

        var response = await client.PostAsJsonAsync($"{baseUrl}/admin/realms/{realm}/users", userRegistrationDto);
        
        var userId = response.Headers.Location?.ToString().Split('/').Last()!;
        
        // TODO - get clientId from appsettings.json
        var clientObject = await client.GetAsync($"{baseUrl}/admin/realms/{realm}/clients?clientId=pp-api");
            
        var clientId = (await clientObject.Content.ReadAsStringAsync()).Split('"')[3];
        

        var rolesObject = await client.GetAsync($"{baseUrl}/admin/realms/{realm}/clients/{clientId}/roles/{role}");
        
        var roles = await rolesObject.Content.ReadFromJsonAsync<RoleDto>()
                    ?? throw new KeycloakNullResponseException();
        
        var jsonContent = JsonContent.Create(new[]
        {
            new RoleDto
            {
                Id = roles.Id,
                Name = roles.Name,
                Description = roles.Description,
                Composite = roles.Composite,
                ClientRole = roles.ClientRole,
                ContainerId = roles.ContainerId,
                Attributes = roles.Attributes
            }
        });
        
        var assignRoles = await client.PostAsync($"{baseUrl}/admin/realms/{realm}/users/{userId}/role-mappings/clients/{clientId}", jsonContent);
        
        return response;
    }
    
    private async Task<KeycloakRegistrationResponse> GetAccessTokenAsync()
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

        var response =
            await client.PostAsync($"{baseUrl}/realms/{realm}/protocol/openid-connect/token", content);

        var mappedResponse =
            await response.Content.ReadFromJsonAsync<KeycloakRegistrationResponse>()
            ?? throw new KeycloakNullResponseException();

        return mappedResponse;
    }
}
