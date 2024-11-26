using System.Net.Http.Json;
using AutoMapper;
using Microsoft.Extensions.Options;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.Configuration;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services;

public class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IOptions<KeycloakConfig> keycloakConfig,
    IKeycloakService keycloakService) : IUserService
{
    public async Task<ServiceResult<StudentDto>> AddUser(StudentUpsertDto studentUpsertDto)
    {
        var student = mapper.Map<Student>(studentUpsertDto);

        var userRegistrationDto = mapper.Map<UserRegistrationDto>(studentUpsertDto);

        if (!await unitOfWork.Nationalities.ExistsAsync(studentUpsertDto.NationalityId))
            return ServiceResult<StudentDto>.Failure(["Invalid Nationality Id"]);

        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;

        unitOfWork.Students.Add(student);

        if (!await unitOfWork.SaveChangesAsync())
            return ServiceResult<StudentDto>.Failure(["Fail to save to database from UserService"]);

        var accessTokenResponse = await keycloakService.GetAccessTokenAsync();

        var client = new HttpClient();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessTokenResponse.Token);

        var response =
            await client.PostAsJsonAsync($"{baseUrl}/admin/realms/{realm}/users", userRegistrationDto);

        if (!response.IsSuccessStatusCode)
        {
            unitOfWork.Students.Delete(student);

            await unitOfWork.SaveChangesAsync();

            return ServiceResult<StudentDto>.Failure([await response.Content.ReadAsStringAsync()]);
        }

        var studentDto = mapper.Map<StudentDto>(student);

        return ServiceResult<StudentDto>.Success(studentDto);
    }
}