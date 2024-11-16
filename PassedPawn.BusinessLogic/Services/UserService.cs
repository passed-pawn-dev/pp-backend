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

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IOptions<KeycloakConfig> keycloakConfig) : IUserService
{
    public async Task<KeyclockRegistrationResponse> GetAccessTokenAsync()
    {
        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;
        var clientId = keycloakConfig.Value.ClientId;
        var clientSecret = keycloakConfig.Value.ClientSecret; 
        var grandType = keycloakConfig.Value.GrandType;

        if (baseUrl is null || realm is null || clientId is null || clientSecret is null || grandType is null)
            throw new NullReferenceException("Invalid Keycloak configuration");
        
        var formData = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", grandType }
        };
        
        var content = new FormUrlEncodedContent(formData);
        
        var client = new HttpClient();
        
        HttpResponseMessage response = await client.PostAsync($"{baseUrl}/realms/{realm}/protocol/openid-connect/token", content);

        var mappedResponse = await response.Content.ReadFromJsonAsync<KeyclockRegistrationResponse>();

        if (mappedResponse is null)
            throw new KeycloakNullResponseException();

        return mappedResponse;
    }

    public async Task<ServiceResult<HttpResponseMessage>> AddUser(StudentUpsertDto studentUpsertDto)
    {
        
        var student = mapper.Map<Student>(studentUpsertDto);
        
        var userRegistrationDto = mapper.Map<UserRegistrationDto>(studentUpsertDto);
        
        if (!await unitOfWork.Nationalities.ExistsAsync(studentUpsertDto.NationalityId))
        {
            return new ServiceResult<HttpResponseMessage>()
            {
                Data = null,
                Errors = ["Invalid Nationality Id"]
            };
        }

        var baseUrl = keycloakConfig.Value.BaseUrl;
        var realm = keycloakConfig.Value.Realm;
        
        unitOfWork.Students.Add(student);
        
        if (!await unitOfWork.SaveChangesAsync())
        {
            return new ServiceResult<HttpResponseMessage>()
            {
                Data = null,
                Errors = ["Fail to save to database from UserService"]
            };
        }

        var accessTokenResponse = await GetAccessTokenAsync();
        
        var client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessTokenResponse.Token);

        HttpResponseMessage response = await client.PostAsJsonAsync($"{baseUrl}/admin/realms/{realm}/users", userRegistrationDto);

        if (!response.IsSuccessStatusCode)
        {
            unitOfWork.Students.Delete(student);

            await unitOfWork.SaveChangesAsync();
            
            return new ServiceResult<HttpResponseMessage>()
            {
                Data = null,
                Errors = [await response.Content.ReadAsStringAsync()]
            };
        }
        
        return new ServiceResult<HttpResponseMessage>()
        {
            Data = response
        };
        
    }
}