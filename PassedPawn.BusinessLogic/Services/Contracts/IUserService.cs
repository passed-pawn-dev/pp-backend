using PassedPawn.Models;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IUserService
{
    public Task<KeyclockRegistrationResponse> GetAccessTokenAsync();
    
    public Task<ServiceResult<HttpResponseMessage>> AddUser(StudentUpsertDto student);
}