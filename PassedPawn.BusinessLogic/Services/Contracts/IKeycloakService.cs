using PassedPawn.Models.DTOs.User;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IKeycloakService
{
    public Task<HttpResponseMessage> RegisterUserInKeycloak<T>(T dto) where T : UserUpsertDto;
}