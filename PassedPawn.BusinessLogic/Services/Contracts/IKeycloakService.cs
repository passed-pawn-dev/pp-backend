using PassedPawn.Models.DTOs.Keycloak;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IKeycloakService
{
    public Task<KeycloakRegistrationResponse> GetAccessTokenAsync();
}