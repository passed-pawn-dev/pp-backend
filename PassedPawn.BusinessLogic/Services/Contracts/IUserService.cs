using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Coach;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IUserService
{
    public Task<ServiceResult<StudentDto>> AddStudent(StudentUpsertDto student);
    public Task<ServiceResult<CoachDto>> AddCoach(CoachUpsertDto coach);

}