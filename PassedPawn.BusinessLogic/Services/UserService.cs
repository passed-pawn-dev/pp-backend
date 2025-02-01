using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Coach;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services;

public class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IKeycloakService keycloakService) : IUserService
{
    public async Task<ServiceResult<StudentDto>> AddStudent(StudentUpsertDto studentUpsertDto)
    {
        var student = mapper.Map<Student>(studentUpsertDto);

        if (!await unitOfWork.Nationalities.ExistsAsync(studentUpsertDto.NationalityId))
            return ServiceResult<StudentDto>.Failure(["Invalid Nationality Id"]);

        unitOfWork.Students.Add(student);

        if (!await unitOfWork.SaveChangesAsync())
            return ServiceResult<StudentDto>.Failure(["Fail to save to database from UserService"]);

        var response = await keycloakService.RegisterUserInKeycloak(studentUpsertDto);

        if (!response.IsSuccessStatusCode)
        {
            unitOfWork.Students.Delete(student);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<StudentDto>.Failure([await response.Content.ReadAsStringAsync()]);
        }

        var studentDto = mapper.Map<StudentDto>(student);
        return ServiceResult<StudentDto>.Success(studentDto);
    }

    public async Task<ServiceResult<CoachDto>> AddCoach(CoachUpsertDto coachUpsertDto)
    {
        var coach = mapper.Map<Coach>(coachUpsertDto);

        if (!await unitOfWork.Nationalities.ExistsAsync(coachUpsertDto.NationalityId))
            return ServiceResult<CoachDto>.Failure(["Invalid Nationality Id"]);

        unitOfWork.Coaches.Add(coach);

        if (!await unitOfWork.SaveChangesAsync())
            return ServiceResult<CoachDto>.Failure(["Fail to save to database from UserService"]);

        var response = await keycloakService.RegisterUserInKeycloak(coachUpsertDto);

        if (!response.IsSuccessStatusCode)
        {
            unitOfWork.Coaches.Delete(coach);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<CoachDto>.Failure([await response.Content.ReadAsStringAsync()]);
        }

        var coachDto = mapper.Map<CoachDto>(coach);
        return ServiceResult<CoachDto>.Success(coachDto);
    }
}
