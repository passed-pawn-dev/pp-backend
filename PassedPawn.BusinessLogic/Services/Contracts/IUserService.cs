using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IUserService
{
    public Task<ServiceResult<StudentDto>> AddUser(StudentUpsertDto student);
}