using System.Net.Http.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PassedPawn.BusinessLogic.Exceptions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.Configuration;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<KeycloakConfig> keycloakConfig, IKeycloakService keycloakService) : IUserService
{
    public async Task<ServiceResult<HttpResponseMessage>> AddUser(StudentUpsertDto studentUpsertDto)
    {
        
        var student = mapper.Map<Student>(studentUpsertDto);
        
        var userRegistrationDto = mapper.Map<UserRegistrationDto>(studentUpsertDto);
        
        if (!await unitOfWork.Nationalities.ExistsAsync(studentUpsertDto.NationalityId))
        {
            return ServiceResult<HttpResponseMessage>.Failure(["Invalid Nationality Id"]);
        }

        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;
        
        unitOfWork.Students.Add(student);
        
        if (!await unitOfWork.SaveChangesAsync())
        {
            return ServiceResult<HttpResponseMessage>.Failure(["Fail to save to database from UserService"]);
        }

        var accessTokenResponse = await keycloakService.GetAccessTokenAsync();
        
        var client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessTokenResponse.Token);

        HttpResponseMessage response = await client.PostAsJsonAsync($"{baseUrl}/admin/realms/{realm}/users", userRegistrationDto);

        if (!response.IsSuccessStatusCode)
        {
            unitOfWork.Students.Delete(student);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<HttpResponseMessage>.Failure([await response.Content.ReadAsStringAsync()]);
        }

        return ServiceResult<HttpResponseMessage>.Success(response);
    }
}